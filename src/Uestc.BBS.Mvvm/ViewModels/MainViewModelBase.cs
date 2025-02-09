using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

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

        public MainViewModelBase(
            AppSettingModel appSettingModel,
            ILogService logService,
            INotificationService notificationService,
            IDailySentenceService dailySentenceService
        )
        {
            _logService = logService;
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
        }

        public abstract Task DispatcherAsync(Action action);
    }
}
