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
            BoardTabItemModel tabItemModel,
            IncrementalLoadingCollection<TopicOverviewSource, TopicOverview> topics
        )> BoardTabItemModels { get; init; } =
            [
                .. appSettingModel.Apperance.BoardTabItems.Select(b =>
                    (
                        b,
                        new IncrementalLoadingCollection<TopicOverviewSource, TopicOverview>(
                            new TopicOverviewSource(topicService, b)
                        )
                    )
                )
            ];

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
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
                .ContinueWith(t => t.Result?.List ?? []);
    }
}
