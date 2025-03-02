using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Core.Services.Forum.TopicList;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class HomeViewModelBase(
        ILogService logService,
        ITopicService topicService,
        ITopicListService topicListService,
        AppSettingModel appSettingModel
    ) : ObservableObject
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogService _logService = logService;

        /// <summary>
        /// 主题内容
        /// </summary>
        protected readonly ITopicService _topicListService = topicService;

        /// <summary>
        /// 主题列表
        /// </summary>
        protected readonly ITopicListService _topicService = topicListService;

        /// <summary>
        /// 应用配置
        /// </summary>
        public AppSettingModel AppSettingModel { get; init; } = appSettingModel;

        /// <summary>
        /// 当前选中的 Tab 栏
        /// </summary>
        [ObservableProperty]
        public partial BoardTabItemModel? CurrentBoardTabItemModel { get; set; } =
            appSettingModel.Appearance.BoardTab.Items.First();

        [ObservableProperty]
        public partial TopicOverview? SeletedTopicOverview { get; set; }

        protected virtual async Task<TopicOverview[]> LoadTopicsAsync(
            BoardTabItemModel tabItem,
            bool IsRefresh = false
        ) =>
            await _topicService
                .GetTopicsAsync(
                    route: tabItem.Route,
                    page: (uint)tabItem.Topics.Count / tabItem.PageSize + 1,
                    pageSize: tabItem.PageSize,
                    moduleId: tabItem.ModuleId,
                    boardId: tabItem.Board,
                    sortby: tabItem.SortType,
                    getPreviewSources: tabItem.RequirePreviewSources
                )
                .ContinueWith(t => t.Result?.List ?? []);

        public abstract Task DispatcherAsync(Action action);
    }
}
