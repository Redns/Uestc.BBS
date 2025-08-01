using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Core.Services.Forum.TopicList;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public partial class HomeViewModelBase : ObservableObject
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogService _logService;

        /// <summary>
        /// 主题内容
        /// </summary>
        protected readonly ITopicService _topicService;

        /// <summary>
        /// 主题列表
        /// </summary>
        protected readonly ITopicListService _topicListService;

        /// <summary>
        /// 应用配置
        /// </summary>
        public AppSettingModel AppSettingModel { get; init; }

        /// <summary>
        /// 最后选中的 Tab 栏
        /// </summary>
        public BoardTabItemModel? LastBoardTabItemModel { get; private set; }

        /// <summary>
        /// 当前选中的 Tab 栏
        /// </summary>
        [ObservableProperty]
        public partial BoardTabItemModel? CurrentBoardTabItemModel { get; set; }

        /// <summary>
        /// 选中的主题帖
        /// </summary>
        [ObservableProperty]
        public partial TopicOverview? SeletedTopicOverview { get; set; }

        public HomeViewModelBase(
            ILogService logService,
            ITopicService topicService,
            ITopicListService topicListService,
            AppSettingModel appSettingModel
        )
        {
            _logService = logService;
            _topicService = topicService;
            _topicListService = topicListService;

            AppSettingModel = appSettingModel;
            CurrentBoardTabItemModel = appSettingModel.Appearance.BoardTab.Items.First();

            PropertyChanging += (_, e) =>
            {
                if (e.PropertyName == nameof(CurrentBoardTabItemModel))
                {
                    LastBoardTabItemModel = CurrentBoardTabItemModel;
                    return;
                }
            };
        }

        /// <summary>
        /// 加载主题列表
        /// </summary>
        /// <param name="tabItem"></param>
        /// <param name="IsRefresh"></param>
        /// <returns></returns>
        protected virtual async Task<TopicOverview[]> LoadTopicsAsync(
            BoardTabItemModel tabItem,
            bool IsRefresh = false
        ) =>
            await _topicListService
                .GetTopicsAsync(
                    route: tabItem.Route,
                    page: (uint)tabItem.Topics.Count / tabItem.PageSize + 1,
                    pageSize: tabItem.PageSize,
                    moduleId: tabItem.ModuleId,
                    boardId: tabItem.Board,
                    sortby: tabItem.SortType,
                    getPreviewSources: tabItem.RequirePreviewSources
                )
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        _logService.Error("Load topics failed", t.Exception.InnerException!);
                    }
                    return t.Result?.List ?? [];
                });
    }
}
