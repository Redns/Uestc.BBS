using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class BoardTabItemListView : UserControl
    {
        private static readonly ITopicService _topicService =
            ServiceExtension.Services.GetRequiredService<ITopicService>();

        /// <summary>
        ///
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

                Topics = new IncrementalLoadingCollection<TopicOverviewSource, TopicOverview>(
                    new TopicOverviewSource(_topicService, value)
                );

                if (IsStaggeredLayoutEnabled)
                {
                    Topics.RefreshAsync();
                }
            }
        }

        /// <summary>
        /// 是否启用瀑布流布局
        /// </summary>
        private static readonly DependencyProperty IsStaggeredLayoutEnabledProperty =
            DependencyProperty.Register(
                nameof(IsStaggeredLayoutEnabled),
                typeof(bool),
                typeof(BoardTabItemListView),
                new PropertyMetadata(false)
            );

        public bool IsStaggeredLayoutEnabled
        {
            get => (bool)GetValue(IsStaggeredLayoutEnabledProperty);
            set => SetValue(IsStaggeredLayoutEnabledProperty, value);
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        public IncrementalLoadingCollection<TopicOverviewSource, TopicOverview>? Topics
        {
            get;
            private set;
        }

        public BoardTabItemListView()
        {
            InitializeComponent();
        }

        public void LoadMoreData(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sender is not ScrollViewer scrollViewer)
            {
                return;
            }

            var offset = scrollViewer.VerticalOffset;
            var viewport = scrollViewer.ViewportHeight;
            var extent = scrollViewer.ExtentHeight;

            if (extent - offset <= viewport)
            {
                Debug.WriteLine("Load more data");
                Topics?.LoadMoreItemsAsync(BoardTabItem.PageSize);
            }
        }
    }

    public class TopicOverviewSource(ITopicService topicService, BoardTabItemModel boardTabItem)
        : IIncrementalSource<TopicOverview>
    {
        private readonly ITopicService _topicService = topicService;

        private readonly BoardTabItemModel boardTabItem = boardTabItem;

        public async Task<IEnumerable<TopicOverview>> GetPagedItemsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default
        ) =>
            await _topicService
                .GetTopicsAsync(
                    route: boardTabItem.Route,
                    page: (uint)pageIndex + 1,
                    pageSize: (uint)pageSize,
                    moduleId: boardTabItem.ModuleId,
                    boardId: boardTabItem.Board,
                    sortby: boardTabItem.SortType,
                    getPreviewSources: boardTabItem.RequirePreviewSources,
                    cancellationToken: cancellationToken
                )
                .ContinueWith(t =>
                    t.Result?.List.DistinctBy(t =>
                            boardTabItem.Board is Board.Hot ? t.SourceId : t.TopicId
                        )
                        .Select(t =>
                        {
                            t.IsHot = t.Hot > 0 || boardTabItem.Board is Board.Hot;
                            return t;
                        }) ?? []
                );
    }
}
