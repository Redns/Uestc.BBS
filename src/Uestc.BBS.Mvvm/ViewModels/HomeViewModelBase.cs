using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.Thread.ThreadContent;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;
using Uestc.BBS.Sdk.Services.Thread.ThreadReply;

namespace Uestc.BBS.Mvvm.ViewModels
{
    /// <summary>
    /// 主页面 ViewModel 基类
    /// </summary>
    /// <typeparam name="IBoardTabItemListView">板块对应的主题列表 View</typeparam>
    public partial class HomeViewModelBase<IBoardTabItemListView> : ObservableObject
        where IBoardTabItemListView : class
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogService _logService;

        /// <summary>
        /// 通知
        /// </summary>
        protected readonly INotificationService _notificationService;

        /// <summary>
        /// 主题列表
        /// </summary>
        protected readonly IThreadListService _threaListService;

        /// <summary>
        /// 主题内容
        /// </summary>
        protected readonly IThreadContentService _threadContentService;

        /// <summary>
        /// 主题回复
        /// </summary>
        protected readonly IThreadReplyService _threadReplyService;

        /// <summary>
        /// Tab 栏对应的 View 列表
        /// </summary>
        protected List<IBoardTabItemListView> BoardTabItemListViewList
        {
            get =>
                field ??= [
                    .. AppSettingModel.Appearance.BoardTab.Items.Select(b =>
                        BoardTabItemModelToView(b)
                    ),
                ];
        }

        /// <summary>
        /// 根据 BoardTabItemModel 生成对应 View
        /// </summary>
        public required Func<
            BoardTabItemModel,
            IBoardTabItemListView
        > BoardTabItemModelToView { get; init; }

        /// <summary>
        /// 获取 View 对应 BoardTabItemModel
        /// </summary>
        public required Func<
            IBoardTabItemListView,
            BoardTabItemModel
        > BoardTabItemModelFromView { get; init; }

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
            BoardTabItemListViewList.FirstOrDefault(b =>
                BoardTabItemModelFromView(b) == CurrentBoardTabItemModel
            ) ?? BoardTabItemListViewList[0];

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
        /// TODO 使用 Converter 优化
        /// </summary>
        public bool IsCurrentThreadFromUs =>
            CurrentThread?.Uid == AppSettingModel.Account.DefaultCredentialUid;

        /// <summary>
        /// 评论内容
        /// </summary>
        [ObservableProperty]
        public partial string ReplyContent { get; set; } = string.Empty;

        public HomeViewModelBase(
            ILogService logService,
            INotificationService notificationService,
            IThreadListService threadListService,
            IThreadContentService threadContentService,
            IThreadReplyService threadReplyService,
            AppSettingModel appSettingModel
        )
        {
            _logService = logService;
            _notificationService = notificationService;
            _threaListService = threadListService;
            _threadContentService = threadContentService;
            _threadReplyService = threadReplyService;

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

        [RelayCommand]
        public async Task ReplyAsync()
        {
            if (CurrentThread is null || string.IsNullOrEmpty(ReplyContent))
            {
                return;
            }

            try
            {
                await _threadReplyService
                    .SendAsync(ReplyContent, CurrentThread.Id)
                    .TimeoutCancelAsync(TimeSpan.FromSeconds(10));

                ReplyContent = string.Empty;
            }
            catch (TaskCanceledException)
            {
                _notificationService.Show("回复失败", "网络超时请稍后重试");
            }
            catch (Exception ex)
            {
                _notificationService.Show("回复失败", ex.Message);
                _logService.Error("Reply to thread " + CurrentThread.Id + " failed.", ex);
            }
        }

        [RelayCommand]
        private void ClearReplyContent()
        {
            ReplyContent = string.Empty;
        }

        [RelayCommand]
        private void OpenWebsite(string url) => OperatingSystemHelper.OpenWebsite(url);
    }
}
