using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Core.Services.Forum.TopicList;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class BoardTabItemListView : UserControl
    {
        private static readonly ITopicListService _topicListService =
            ServiceExtension.Services.GetRequiredService<ITopicListService>();

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
                Topics = new IncrementalLoadingCollection<TopicOverviewSource, TopicOverview>(
                    new TopicOverviewSource(_topicListService, value)
                );
            }
        }

        /// <summary>
        /// 选中的主题
        /// </summary>
        private static readonly DependencyProperty SelectedTopicProperty =
            DependencyProperty.Register(
                nameof(SelectedTopic),
                typeof(TopicOverview),
                typeof(BoardTabItemListView),
                new PropertyMetadata(null)
            );

        public TopicOverview? SelectedTopic
        {
            get => (TopicOverview)GetValue(SelectedTopicProperty);
            set => SetValue(SelectedTopicProperty, value);
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

            TopicListView.Loaded += (_, _) =>
            {
                var scrollViewer = TopicListView.FindDescendant<ScrollViewer>();
                if (scrollViewer is null)
                {
                    return;
                }

                scrollViewer.ViewChanged += (sender, e) =>
                {
                    if (e.IsIntermediate || sender is not ScrollViewer scrollViewer)
                    {
                        return;
                    }

                    if (Topics?.IsLoading is true)
                    {
                        return;
                    }

                    var offset = scrollViewer.VerticalOffset;
                    var viewport = scrollViewer.ViewportHeight;
                    var extent = scrollViewer.ExtentHeight;

                    if (extent - offset <= 2 * viewport)
                    {
                        Topics?.LoadMoreItemsAsync(BoardTabItem.PageSize);
                    }
                };
            };
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e) =>
            SelectedTopic = e.ClickedItem as TopicOverview;
    }

    public class TopicOverviewSource(
        ITopicListService topicListService,
        BoardTabItemModel boardTabItem
    ) : IIncrementalSource<TopicOverview>
    {
        private readonly ITopicListService _topicListService = topicListService;

        private readonly BoardTabItemModel boardTabItem = boardTabItem;

        public async Task<IEnumerable<TopicOverview>> GetPagedItemsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default
        ) =>
            await _topicListService
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
