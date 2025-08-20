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
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class BoardTabItemListView : UserControl
    {
        private readonly IThreadListService _threaListService =
            ServiceExtension.Services.GetRequiredService<IThreadListService>();

        /// <summary>
        /// 当前板块
        /// </summary>
        private static readonly DependencyProperty BoardTabItemProperty =
            DependencyProperty.Register(
                nameof(BoardTabItem),
                typeof(BoardTabItemModel),
                typeof(BoardTabItemListView),
                new PropertyMetadata(null)
            );

        public BoardTabItemModel BoardTabItem
        {
            get => (BoardTabItemModel)GetValue(BoardTabItemProperty);
            set
            {
                SetValue(BoardTabItemProperty, value);
                Topics = new IncrementalLoadingCollection<ThreadOverviewSource, ThreadOverview>(
                    new ThreadOverviewSource(_threaListService, value),
                    itemsPerPage: 30
                );
            }
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        public IncrementalLoadingCollection<ThreadOverviewSource, ThreadOverview>? Topics
        {
            get;
            private set;
        }

        public BoardTabItemListView()
        {
            InitializeComponent();
        }

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
        BoardTabItemModel boardTabItem
    ) : IIncrementalSource<ThreadOverview>
    {
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

            return threads
                .DistinctBy(o => o.Id)
                .Select(o =>
                {
                    // XXX 热门板块的时间为发布时间，其余为最新回复时间
                    // 由于热门板块实际上是由各板块主题组成，因此 Board 字段不是 Hot 而是其对应的板块名称
                    // 因此这里需要判断板块是否为热门板块，并设置 IsHot 属性
                    o.IsHot = boardTabItem.Board is Board.Hot;
                    // 设置匿名用户头像
                    o.UserAvatar = o.IsAnonymous
                        ? "ms-appx:///Assets/Icons/anonymous.png"
                        : o.UserAvatar;
                    return o;
                });
        }
    }
}
