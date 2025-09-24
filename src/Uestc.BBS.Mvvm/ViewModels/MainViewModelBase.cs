using System.Security.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Entities;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.Sdk.Services.Auth;
using Uestc.BBS.Sdk.Services.System;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.User.Friend;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class MainViewModelBase<TContent> : ObservableObject
        where TContent : class
    {
        #region Timer & Token
        /// <summary>
        /// 黑名单定时更新
        /// </summary>
        protected readonly Timer _blacklistUpdateTimer;

        /// <summary>
        /// 全站状态定时更新
        /// </summary>
        protected readonly Timer _globalStatusUpdateTimer;

        /// <summary>
        /// 搜索栏文字定时更新
        /// </summary>
        protected readonly Timer _searchPlaceholderTextUpdateTimer;

        /// <summary>
        /// 黑名单 CancellationTokenSource
        /// </summary>
        protected CancellationTokenSource? _blaclistCancelTokenSource;

        /// <summary>
        /// 全站状态 CancellationTokenSource
        /// </summary>
        protected CancellationTokenSource? _globalStatusCancelTokenSource;

        /// <summary>
        /// 每日一句 CancellationTokenSource
        /// </summary>
        protected CancellationTokenSource? _daiysentenceCancelTokenSource;
        #endregion

        #region Services
        /// <summary>
        /// 日志服务
        /// </summary>
        protected readonly ILogService _logService;

        /// <summary>
        /// 好友列表服务
        /// </summary>
        protected readonly IFriendListService _friendListService;

        /// <summary>
        /// 通知服务
        /// </summary>
        protected readonly INotificationService _notificationService;

        /// <summary>
        /// 全站状态服务
        /// </summary>
        protected readonly IGlobalStatusService _globalStatusService;

        /// <summary>
        /// 每日一句
        /// </summary>
        protected readonly IDailySentenceService _dailySentenceService;

        /// <summary>
        /// 导航服务
        /// </summary>
        protected readonly INavigateService<TContent> _navigateService;
        #endregion

        /// <summary>
        /// 全局状态
        /// </summary>
        [ObservableProperty]
        public partial GlobalStatusModel GlobalStatus { get; set; } = new();

        /// <summary>
        /// 搜索栏提示文字
        /// </summary>
        public string SearchPlaceholderText
        {
            get =>
                AppSettingModel.Appearance.SearchBar.IsDailySentenceEnabled ? field : string.Empty;
            set => SetProperty(ref field, value);
        } = "韶光易逝，劝君惜取少年时";

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
            IFriendListService friendListService,
            IGlobalStatusService globalStatusService,
            INotificationService notificationService,
            IDailySentenceService dailySentenceService,
            INavigateService<TContent> navigateService,
            Repository<ThreadHistoryEntity> threadHistoryRepository
        )
        {
            _logService = logService;
            _friendListService = friendListService;
            _navigateService = navigateService;
            _globalStatusService = globalStatusService;
            _notificationService = notificationService;
            _dailySentenceService = dailySentenceService;

            _blacklistUpdateTimer = new Timer(
                async _ =>
                {
                    _blaclistCancelTokenSource?.Cancel();
                    _blaclistCancelTokenSource?.Dispose();
                    _blaclistCancelTokenSource = new CancellationTokenSource();

                    try
                    {
                        if (appSettingModel.Account.DefaultCredential is null)
                        {
                            return;
                        }

                        uint page = 1;
                        var blacklistUsers = new List<BlacklistUser>(128);

                        while (true)
                        {
                            var pageBlacklistUsers = await _friendListService
                                .GetFriendListAsync(
                                    FriendType.Blacklist,
                                    page++,
                                    cancellationToken: _blaclistCancelTokenSource.Token
                                )
                                .TimeoutCancelAsync(TimeSpan.FromSeconds(5));

                            if (!pageBlacklistUsers.Any())
                            {
                                break;
                            }

                            blacklistUsers.AddRange(pageBlacklistUsers);
                        }

                        if (
                            blacklistUsers
                                .Select(u => u.Uid)
                                .ToHashSet()
                                .SetEquals(
                                    appSettingModel.Account.DefaultCredential.BlacklistUsers.Select(
                                        u => u.Uid
                                    )
                                )
                        )
                        {
                            return;
                        }

                        await DispatcherAsync(() =>
                        {
                            appSettingModel.Account.DefaultCredential!.BlacklistUsers.Clear();
                            blacklistUsers.ForEach(u =>
                                appSettingModel.Account.DefaultCredential.BlacklistUsers.Add(u)
                            );
                        });
                    }
                    catch (TimeoutException) { }
                    catch (TaskCanceledException) { }
                    catch (AuthenticationException)
                    {
                        // TODO 登录失效
                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Blacklist update failed", ex);
                    }
                },
                null,
                5 * 1000,
                60 * 1000
            );
            _globalStatusUpdateTimer = new Timer(
                async _ =>
                {
                    _globalStatusCancelTokenSource?.Cancel();
                    _globalStatusCancelTokenSource?.Dispose();
                    _globalStatusCancelTokenSource = new CancellationTokenSource();

                    try
                    {
                        var globalStatus = await _globalStatusService.GetGlobalStatusAsync(
                            _globalStatusCancelTokenSource.Token
                        );

                        await DispatcherAsync(() =>
                        {
                            GlobalStatus.TodayPostCount = globalStatus.TodayPostCount;
                            GlobalStatus.YesterdayPostCount = globalStatus.YesterdayPostCount;
                            GlobalStatus.TotalPostCount = globalStatus.TotalPostCount;
                            GlobalStatus.TotalUserCount = globalStatus.TotalUserCount;
                        });
                    }
                    catch (TimeoutException) { }
                    catch (TaskCanceledException) { }
                    catch (Exception ex)
                    {
                        _logService.Error("Global status update failed", ex);
                    }
                },
                null,
                0,
                300 * 1000
            );
            _searchPlaceholderTextUpdateTimer = new Timer(
                async _ =>
                {
                    if (!appSettingModel.Appearance.SearchBar.IsDailySentenceEnabled)
                    {
                        return;
                    }

                    _daiysentenceCancelTokenSource?.Cancel();
                    _daiysentenceCancelTokenSource?.Dispose();
                    _daiysentenceCancelTokenSource = new CancellationTokenSource();

                    try
                    {
                        var sentence = await _dailySentenceService
                            .GetDailySentenceAsync(_daiysentenceCancelTokenSource.Token)
                            .TimeoutCancelAsync(TimeSpan.FromSeconds(60));
                        if (string.IsNullOrEmpty(sentence) || sentence == SearchPlaceholderText)
                        {
                            return;
                        }
                        await DispatcherAsync(
                            () =>
                                SearchPlaceholderText = sentence.TrimEnd(
                                    ['，', '；', '。', ',', ';', '.']
                                )
                        );
                    }
                    catch (TimeoutException) { }
                    catch (TaskCanceledException) { }
                    catch (Exception ex)
                    {
                        _logService.Error("Daily sentence uppdate failed", ex);
                    }
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

            StrongReferenceMessenger.Default.Register<ThreadHistoryChangedMessage>(
                this,
                async (_, m) =>
                {
                    var threadOverview = m.Value;
                    // TODO 使用 Mapster 源码生成器
                    var ret = await threadHistoryRepository.AddOrUpdateAsync(
                        new ThreadHistoryEntity
                        {
                            Id = threadOverview.Id,
                            BoardName = threadOverview.BoardName,
                            Title = threadOverview.Title,
                            Subject = threadOverview.Subject,
                            Uid = threadOverview.Uid,
                            Username = threadOverview.Username,
                            UserAvatar = threadOverview.UserAvatar,
                            HasVote = threadOverview.HasVote,
                            BrowserDateTime = DateTime.Now,
                        }
                    );

                    if (ret is 0)
                    {
                        _logService.Error("Thread history add or update failed.");
                    }
                }
            );
        }

        ~MainViewModelBase()
        {
            _blacklistUpdateTimer.Dispose();
            _globalStatusUpdateTimer.Dispose();
            _searchPlaceholderTextUpdateTimer.Dispose();
            StrongReferenceMessenger.Default.Unregister<NavigateChangedMessage>(this);
        }

        /// <summary>
        /// 返回上一页
        /// </summary>
        [RelayCommand]
        private void NavigateBack() => CurrentMenuKey = CurrentMenuItem.Key;

        public abstract Task DispatcherAsync(Action action);
    }
}
