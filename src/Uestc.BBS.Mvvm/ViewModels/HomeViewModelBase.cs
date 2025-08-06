using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Core.Services.Forum.TopicList;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    /// <summary>
    /// 主页面 ViewModel 基类
    /// </summary>
    /// <typeparam name="IBoardTabItemListView">板块对应的主题列表 View</typeparam>
    public partial class HomeViewModelBase<IBoardTabItemListView> : ObservableObject
        where IBoardTabItemListView : new()
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
        /// Tab 栏对应的 View 列表
        /// </summary>
        protected readonly List<IBoardTabItemListView> _boardTabItemListViewList;

        /// <summary>
        /// 根据 BoardTabItemModel 生成对应 View
        /// </summary>
        protected readonly Func<BoardTabItemModel, IBoardTabItemListView> _boardTabItemModelToView;

        /// <summary>
        /// 获取 View 对应 BoardTabItemModel
        /// </summary>
        protected readonly Func<
            IBoardTabItemListView,
            BoardTabItemModel
        > _boardTabItemModelFromView;

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
        [NotifyPropertyChangedFor(nameof(CurrentBoardTabItemListView))]
        public partial BoardTabItemModel? CurrentBoardTabItemModel { get; set; }

        /// <summary>
        /// 当前选中的 Tab 栏的 View
        /// </summary>
        public IBoardTabItemListView CurrentBoardTabItemListView =>
            _boardTabItemListViewList.FirstOrDefault(b =>
                _boardTabItemModelFromView(b) == CurrentBoardTabItemModel
            ) ?? _boardTabItemListViewList[0];

        /// <summary>
        /// 选中的主题帖
        /// </summary>
        [ObservableProperty]
        public partial TopicOverview? SeletedTopicOverview { get; set; }

        public HomeViewModelBase(
            ILogService logService,
            ITopicService topicService,
            ITopicListService topicListService,
            Func<BoardTabItemModel, IBoardTabItemListView> boardTabItemModelToView,
            Func<IBoardTabItemListView, BoardTabItemModel> boardTabItemModelFromView,
            AppSettingModel appSettingModel
        )
        {
            _logService = logService;
            _topicService = topicService;
            _topicListService = topicListService;
            _boardTabItemModelToView = boardTabItemModelToView;
            _boardTabItemModelFromView = boardTabItemModelFromView;
            _boardTabItemListViewList =
            [
                .. appSettingModel.Appearance.BoardTab.Items.Select(b =>
                    boardTabItemModelToView(b)
                ),
            ];

            AppSettingModel = appSettingModel;
            CurrentBoardTabItemModel = appSettingModel.Appearance.BoardTab.Items.First();

            PropertyChanging += (_, e) =>
            {
                // 记录上一次选中的 Tab 栏
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
                    page: IsRefresh ? 1 : (uint)tabItem.Topics.Count / tabItem.PageSize + 1,
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
