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
                    new ThreadOverviewSource(_threaListService, value)
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
        public Task<IEnumerable<ThreadOverview>> GetPagedItemsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default
        ) =>
            threadListService
                .GetThreadListAsync(
                    route: boardTabItem.Route,
                    page: (uint)pageIndex + 1,
                    pageSize: (uint)pageSize,
                    moduleId: boardTabItem.ModuleId,
                    boardId: boardTabItem.Board,
                    sortby: boardTabItem.SortType,
                    getPreviewSources: boardTabItem.RequirePreviewSources,
                    cancellationToken: cancellationToken
                )
                .ContinueWith(
                    t =>
                        // TODO 部分板块加载时会出现重复主题，需要去重
                        t.Result.DistinctBy(o => o.Id)
                            .Select(o =>
                            {
                                o.IsHot = boardTabItem.Board is Board.Hot;
                                return o;
                            }),
                    cancellationToken
                );
    }
}
