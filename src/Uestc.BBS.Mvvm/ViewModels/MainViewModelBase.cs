using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.Sdk.Services.System;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class MainViewModelBase<TContent> : ObservableObject
        where TContent : class
    {
        /// <summary>
        /// 搜索栏文字定时更新
        /// </summary>
        protected readonly Timer _searchPlaceholderTextUpdateTimer;

        /// <summary>
        /// 日志服务
        /// </summary>
        protected readonly ILogService _logService;

        /// <summary>
        /// 通知服务
        /// </summary>
        protected readonly INotificationService _notificationService;

        /// <summary>
        /// 每日一句
        /// </summary>
        protected readonly IDailySentenceService _dailySentenceService;

        /// <summary>
        /// 导航服务
        /// </summary>
        protected readonly INavigateService<TContent> _navigateService;

        /// <summary>
        /// 搜索栏提示文字
        /// </summary>
        public string SearchPlaceholderText
        {
            get =>
                AppSettingModel.Appearance.SearchBar.IsDailySentenceEnabled ? field : string.Empty;
            set => SetProperty(ref field, value);
        } = string.Empty;

        /// <summary>
        /// 应用设置
        /// </summary>
        public AppSettingModel AppSettingModel { get; init; }

        /// <summary>
        /// 当前选中的菜单项
        /// </summary>
        public MenuItemModel CurrentMenuItem
        {
            get => field;
            set
            {
                SetProperty(ref field, value);
                CurrentMenuKey = value.Key;
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentMenuContent))]
        [NotifyPropertyChangedFor(nameof(IsBackButtonEnabled))]
        public partial MenuItemKey CurrentMenuKey { get; set; }

        /// <summary>
        /// 当前页面的视图模型
        /// </summary>
        public TContent CurrentMenuContent => _navigateService.Navigate(CurrentMenuKey);

        /// <summary>
        /// 是否显示返回按钮
        /// </summary>
        public bool IsBackButtonEnabled =>
            !AppSettingModel.Appearance.MenuItems.Any(m => m.Key == CurrentMenuKey);

        public MainViewModelBase(
            AppSettingModel appSettingModel,
            ILogService logService,
            INotificationService notificationService,
            IDailySentenceService dailySentenceService,
            INavigateService<TContent> navigateService
        )
        {
            _logService = logService;
            _navigateService = navigateService;
            _notificationService = notificationService;
            _dailySentenceService = dailySentenceService;
            _searchPlaceholderTextUpdateTimer = new Timer(
                _ =>
                {
                    if (!appSettingModel.Appearance.SearchBar.IsDailySentenceEnabled)
                    {
                        return;
                    }

                    _dailySentenceService
                        .GetDailySentenceAsync()
                        .ContinueWith(async t =>
                        {
                            if (t.IsFaulted)
                            {
                                _logService.Error(
                                    "Daily sentence refresh failed",
                                    t.Exception.InnerException!
                                );
                                return;
                            }

                            var sentence = t.Result;
                            if (string.IsNullOrEmpty(sentence) || sentence == SearchPlaceholderText)
                            {
                                return;
                            }
                            await DispatcherAsync(() => SearchPlaceholderText = sentence);
                        });
                },
                null,
                0,
                appSettingModel.Appearance.SearchBar.DailySentenceUpdateTimeInterval * 1000
            );

            AppSettingModel = appSettingModel;
            AppSettingModel.Appearance.SearchBar.PropertyChanged += (_, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(AppearanceSettingModel.SearchBar.IsDailySentenceEnabled):
                        // 更新搜索栏文字
                        OnPropertyChanged(nameof(SearchPlaceholderText));
                        return;
                    case nameof(AppearanceSettingModel.SearchBar.DailySentenceUpdateTimeInterval):
                        // 更新每日一句定时周期
                        _searchPlaceholderTextUpdateTimer.Change(
                            0,
                            AppSettingModel.Appearance.SearchBar.DailySentenceUpdateTimeInterval
                                * 1000
                        );
                        return;
                }
            };

            CurrentMenuItem =
                AppSettingModel.Appearance.MenuItems.FirstOrDefault()
                ?? throw new ArgumentOutOfRangeException(
                    nameof(appSettingModel),
                    "The sidebar must have at least one menu item."
                );

            // 注册导航消息
            StrongReferenceMessenger.Default.Register<NavigateChangedMessage>(
                this,
                (_, m) => CurrentMenuKey = m.Value
            );
        }

        ~MainViewModelBase()
        {
            _searchPlaceholderTextUpdateTimer.Dispose();
            StrongReferenceMessenger.Default.Unregister<NavigateChangedMessage>(this);
        }

        /// <summary>
        /// 返回上一页
        /// </summary>
        [RelayCommand]
        private void NavigateBack() => CurrentMenuKey = CurrentMenuItem.Key;

        public abstract Task DispatcherAsync(Action action);

        /// <summary>
        /// 打开网站
        /// </summary>
        /// <param name="url"></param>
        [RelayCommand]
        private void OpenWebSite(string url) => OperatingSystemHelper.OpenWebsite(url);
    }
}
