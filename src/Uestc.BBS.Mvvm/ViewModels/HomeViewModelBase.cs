using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.Thread.ThreadContent;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;

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
        /// 主题列表
        /// </summary>
        protected readonly IThreadListService _threaListService;

        /// <summary>
        /// 主题内容
        /// </summary>
        protected readonly IThreadContentService _threadContentService;

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
        [NotifyPropertyChangedFor(nameof(IsCurrentThreadFromUs))]
        public partial ThreadContent? CurrentThread { get; set; }

        /// <summary>
        /// 当前正在加载的主题帖 ID
        /// </summary>
        public uint CurrentLoadingThreadId { get; set; }

        /// <summary>
        /// 当前选中的主题帖是否来自于自己
        /// </summary>
        public bool IsCurrentThreadFromUs =>
            CurrentThread?.Uid == AppSettingModel.Account.DefaultCredentialUid;

        public HomeViewModelBase(
            ILogService logService,
            IThreadListService threadListService,
            IThreadContentService threadContentService,
            Func<BoardTabItemModel, IBoardTabItemListView> boardTabItemModelToView,
            Func<IBoardTabItemListView, BoardTabItemModel> boardTabItemModelFromView,
            AppSettingModel appSettingModel
        )
        {
            _logService = logService;
            _threaListService = threadListService;
            _threadContentService = threadContentService;
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
    }
}
