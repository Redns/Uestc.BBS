using System.Collections.Generic;
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
            }
        }

        public IncrementalLoadingCollection<TopicOverviewSource, TopicOverview>? Topics
        {
            get;
            private set;
        }

        public BoardTabItemListView()
        {
            InitializeComponent();
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
                    getPreviewSources: boardTabItem.RequirePreviewSources
                )
                .ContinueWith(t =>
                    boardTabItem.Board == Board.Hot
                        ? t.Result?.List.Select(t =>
                        {
                            t.IsHot = true;
                            return t;
                        }) ?? []
                        : t.Result?.List.DistinctBy(t => t.TopicId)
                            .Select(t =>
                            {
                                t.IsHot = t.Hot > 0;
                                return t;
                            }) ?? []
                );
    }
}
