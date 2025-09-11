using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;

namespace Uestc.BBS.WinUI.Controls
{
    /// <summary>
    /// TODO 重构具体实现
    /// 1. BoardTabItemListView 实现 IBoardTabItemListView 接口，提供 IsLoading 属性、RefreshThreads 方法
    /// 2. 将 HashSet 放置在具体实现类中，采用双列表筛选重复/屏蔽主题
    /// 3. ViewModel 直接注入依赖项，避免使用 ServiceExtension.Services，或者使用 IServiceProvider 注入
    /// </summary>
    public sealed partial class BoardTabItemListView : UserControl
    {
        private readonly IThreadListService _threaListService =
            ServiceExtension.Services.GetRequiredKeyedService<IThreadListService>(
                ServiceExtensions.MOBCENT_API
            );

        private readonly AppSettingModel _appSetting =
            ServiceExtension.Services.GetRequiredService<AppSettingModel>();

        /// <summary>
        /// 当前板块
        /// </summary>
        private static readonly DependencyProperty BoardTabItemProperty =
            DependencyProperty.Register(
                nameof(BoardTabItem),
                typeof(BoardTabItemModel),
                typeof(BoardTabItemListView),
                new PropertyMetadata(
                    null,
                    static (obj, args) =>
                    {
                        if (
                            obj is not BoardTabItemListView view
                            || args.NewValue is not BoardTabItemModel model
                        )
                        {
                            return;
                        }

                        view.Threads = new(
                            new ThreadOverviewSource(
                                view._threaListService,
                                model,
                                t =>
                                {
                                    if (!view._appSetting.Browse.Filter.IsFilterEnable)
                                    {
                                        return true;
                                    }

                                    // 屏蔽投票
                                    if (view._appSetting.Browse.Filter.BlockVote && t.HasVote)
                                    {
                                        return false;
                                    }

                                    // 屏蔽匿名
                                    if (
                                        view._appSetting.Browse.Filter.BlockAnonymousUser
                                        && t.IsAnonymous
                                    )
                                    {
                                        return false;
                                    }

                                    // 无图拦截
                                    // 仅当板块开启了预览图像加载时生效
                                    if (
                                        model.RequirePreviewSources
                                        && view._appSetting.Browse.Filter.BlockNoImage
                                        && t.PreviewImageSources.Length is 0
                                    )
                                    {
                                        return false;
                                    }

                                    // 屏蔽关键词
                                    if (
                                        view._appSetting.Browse.Filter.BlockedKeywords.Any(k =>
                                            t.Title.Contains(k) || t.Subject.Contains(k)
                                        )
                                    )
                                    {
                                        return false;
                                    }

                                    // 屏蔽黑名单
                                    if (
                                        view._appSetting.Account.DefaultCredential?.BlacklistUsers.Any(
                                            u => u.Uid == t.Uid
                                        )
                                        is true
                                    )
                                    {
                                        return false;
                                    }

                                    // 自定义表达式
                                    if (view._appSetting.Browse.Filter.CustomizedFilter(t))
                                    {
                                        return false;
                                    }

                                    return true;
                                }
                            ),
                            itemsPerPage: 30
                        );
                    }
                )
            );

        public BoardTabItemModel BoardTabItem
        {
            get => (BoardTabItemModel)GetValue(BoardTabItemProperty);
            set => SetValue(BoardTabItemProperty, value);
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        private static readonly DependencyProperty ThreadsProperty = DependencyProperty.Register(
            nameof(Threads),
            typeof(IncrementalLoadingCollection<ThreadOverviewSource, ThreadOverview>),
            typeof(BoardTabItemListView),
            new PropertyMetadata(null)
        );

        public IncrementalLoadingCollection<ThreadOverviewSource, ThreadOverview>? Threads
        {
            get =>
                (IncrementalLoadingCollection<ThreadOverviewSource, ThreadOverview>)
                    GetValue(ThreadsProperty);
            private set => SetValue(ThreadsProperty, value);
        }

        public BoardTabItemListView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择主题
        /// </summary>
        /// <param name="_"></param>
        /// <param name="e"></param>
        private void SelectedThreadChanged(object _, ItemClickEventArgs e)
        {
            if (e.ClickedItem is not ThreadOverview threadOverview)
            {
                return;
            }

            StrongReferenceMessenger.Default.Send(new ThreadChangedMessage(threadOverview.Id));
        }
    }

    public class ThreadOverviewSource(
        IThreadListService threadListService,
        BoardTabItemModel boardTabItem,
        Func<ThreadOverview, bool>? filter = null
    ) : IIncrementalSource<ThreadOverview>
    {
        /// <summary>
        /// 已获取的主题 ID 集合
        /// XXX 主题采用分页加载，假设某次加载后新发布若干主题，则下一次加载的主题可能存在重复
        /// </summary>
        private readonly HashSet<uint> _threadIds = new(1024);

        // XXX IIncrementalSource 发生异常时程序崩溃，如果捕获异常并返回 [] 则继续滚动不会获取数据
        // TODO 使用 ScrollViewer 重构
        public async Task<IEnumerable<ThreadOverview>> GetPagedItemsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            var threads = await threadListService.GetThreadListAsync(
                route: boardTabItem.Route,
                page: (uint)pageIndex + 1,
                pageSize: (uint)pageSize,
                moduleId: boardTabItem.ModuleId,
                boardId: boardTabItem.Board,
                sortby: boardTabItem.SortType,
                getPreviewSources: boardTabItem.RequirePreviewSources,
                cancellationToken: cancellationToken
            );

            if (pageIndex is 0)
            {
                _threadIds.Clear();
            }

            return
            [
                .. threads
                    .Where(o => _threadIds.Add(o.Id))
                    .Where(o => filter is null || filter(o))
                    .Select(o =>
                    {
                        // XXX 热门板块的时间为发布时间，其余为最新回复时间
                        // 由于热门板块实际上是由各板块主题组成，因此 Board 字段不是 Hot 而是其对应的板块名称
                        // 因此这里需要判断板块是否为热门板块，并设置 IsHot 属性
                        o.IsHot = boardTabItem.Board is Board.Hot;
                        return o;
                    }),
            ];
        }
    }
}
