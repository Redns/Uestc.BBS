using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Collections;
using Microsoft.UI.Dispatching;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel(
        ILogService logService,
        ITopicService topicService,
        AppSettingModel appSettingModel
    ) : HomeViewModelBase(logService, topicService, appSettingModel)
    {
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public ObservableCollection<(
            BoardTabItemModel tabItem,
            IncrementalLoadingCollection<TopicOverviewSource, TopicOverview> topicOverviews
        )> BoardTabItems { get; init; } =
            [
                .. appSettingModel.Apperance.BoardTabItems.Select(b =>
                    (
                        b,
                        new IncrementalLoadingCollection<TopicOverviewSource, TopicOverview>(
                            new TopicOverviewSource(b, topicService)
                        )
                    )
                ),
            ];

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }

    public class TopicOverviewSource(BoardTabItemModel tabItem, ITopicService topicService)
        : IIncrementalSource<TopicOverview>
    {
        private readonly BoardTabItemModel _tabItem = tabItem;

        private readonly ITopicService _topicService = topicService;

        public async Task<IEnumerable<TopicOverview>> GetPagedItemsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            return await _topicService
                    .GetTopicsAsync(
                        route: _tabItem.Route,
                        page: pageIndex + 1,
                        pageSize: pageSize,
                        boardId: _tabItem.Board,
                        sortby: _tabItem.SortType,
                        moduleId: _tabItem.ModuleId,
                        getPreviewSources: _tabItem.RequirePreviewSources
                    )
                    .ContinueWith(task => task.Result?.List) ?? [];
        }
    }
}
