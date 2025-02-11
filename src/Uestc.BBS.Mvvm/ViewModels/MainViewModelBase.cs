using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class MainViewModelBase : ObservableObject
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
        /// 导航服务
        /// </summary>
        protected readonly INavigateService _navigateService;

        /// <summary>
        /// 通知服务
        /// </summary>
        protected readonly INotificationService _notificationService;

        /// <summary>
        /// 每日一句
        /// </summary>
        protected readonly IDailySentenceService _dailySentenceService;

        /// <summary>
        /// 搜索栏提示文字
        /// </summary>
        public string SearchPlaceholderText
        {
            get =>
                AppSettingModel.Apperance.SearchBar.IsDailySentenceEnabled ? field : string.Empty;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public AppSettingModel AppSettingModel { get; init; }

        /// <summary>
        /// 当前选中的菜单项
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsBackButtonEnabled))]
        [NotifyPropertyChangedFor(nameof(CurrentPageViewModel))]
        public partial MenuItemModel SelectedMenuItem { get; set; }

        /// <summary>
        /// 是否显示返回按钮
        /// </summary>
        public bool IsBackButtonEnabled => SelectedMenuItem.Key != Core.MenuItemKey.Home;

        /// <summary>
        /// 当前页面的视图模型
        /// </summary>
        public ObservableObject CurrentPageViewModel =>
            _navigateService.Navigate(SelectedMenuItem.Key);

        public MainViewModelBase(
            AppSettingModel appSettingModel,
            ILogService logService,
            INavigateService navigateService,
            INotificationService notificationService,
            IDailySentenceService dailySentenceService
        )
        {
            _logService = logService;
            _navigateService = navigateService;
            _notificationService = notificationService;
            _dailySentenceService = dailySentenceService;
            _searchPlaceholderTextUpdateTimer = new Timer(
                async state =>
                {
                    if (!appSettingModel.Apperance.SearchBar.IsDailySentenceEnabled)
                    {
                        return;
                    }

                    try
                    {
                        var sentence = await _dailySentenceService.GetDailySentenceAsync();
                        if (string.IsNullOrEmpty(sentence) || sentence == SearchPlaceholderText)
                        {
                            return;
                        }
                        await DispatcherAsync(() => SearchPlaceholderText = sentence);
                    }
                    catch (Exception e)
                    {
                        _logService.Error("Dailysentence get failed", e);
                        _notificationService.Show("每日一句获取失败", e.Message);
                    }
                },
                null,
                0,
                appSettingModel.Apperance.SearchBar.DailySentenceUpdateTimeInterval * 1000
            );

            AppSettingModel = appSettingModel;
            AppSettingModel.Apperance.SearchBar.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(AppSettingModel.Apperance.SearchBar.IsDailySentenceEnabled):
                        // 更新搜索栏文字
                        OnPropertyChanged(nameof(SearchPlaceholderText));
                        return;
                    case nameof(
                        AppSettingModel.Apperance.SearchBar.DailySentenceUpdateTimeInterval
                    ):
                        // 更新每日一句定时周期
                        _searchPlaceholderTextUpdateTimer.Change(
                            0,
                            AppSettingModel.Apperance.SearchBar.DailySentenceUpdateTimeInterval
                                * 1000
                        );
                        return;
                }
            };

            SelectedMenuItem =
                AppSettingModel.Apperance.MenuItems.FirstOrDefault()
                ?? throw new ArgumentOutOfRangeException(
                    nameof(appSettingModel),
                    "MenuItems is empty"
                );
        }

        public abstract Task DispatcherAsync(Action action);

        [RelayCommand]
        private void OpenWebSite(string url) => OperatingSystemHelper.OpenWebsite(url);
    }
}
