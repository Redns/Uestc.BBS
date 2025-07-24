using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Desktop.Models
{
    public partial class AppSettingModel : ObservableObject
    {
        private readonly IDailySentenceService _dailySentenceService;

        #region 外观
        /// <summary>
        /// 主题色
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TintColor))]
        private ThemeColor _themeColor;

        /// <summary>
        /// 颜色不透明度
        /// </summary>
        [ObservableProperty]
        private double _tintOpacity;

        /// <summary>
        /// 亚克力材质亮色
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TintColor))]
        private Color _tintLightColor;

        /// <summary>
        /// 亚克力主题暗色
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TintColor))]
        private Color _tintDarkColor;

        /// <summary>
        /// 亚克力材质颜色
        /// </summary>
        public Color TintColor =>
            Application.Current!.ActualThemeVariant == ThemeVariant.Light
                ? TintLightColor
                : TintDarkColor;

        /// <summary>
        /// 材质不透明度
        /// </summary>
        [ObservableProperty]
        private double _materialOpacity;

        /// <summary>
        /// 每日一句
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DailySentence))]
        private bool _isDailySentenceShown;

        public Task<string> DailySentence =>
            IsDailySentenceShown
                ? _dailySentenceService
                    .GetDailySentenceAsync()
                    .ContinueWith(sentence =>
                        DailySentenceRegex().Replace(sentence.Result, string.Empty)
                    )
                : Task.FromResult(string.Empty);

        /// <summary>
        /// 论坛官网
        /// </summary>
        [ObservableProperty]
        private string _officialWebsite;

        /// <summary>
        /// 固定窗口
        /// </summary>
        [ObservableProperty]
        private bool _isWindowPinned;

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        [ObservableProperty]
        private WindowCloseBehavior _windowCloseBehavior;
        #endregion

        #region 账号
        [ObservableProperty]
        private bool _autoLogin;

        [ObservableProperty]
        private bool _rememberPassword;
        #endregion

        #region 同步
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SyncTimeIntervalMinutesEnable))]
        private SyncMode _syncMode;

        [ObservableProperty]
        private string _syncSecret;

        [ObservableProperty]
        private double _syncTimeIntervalMinutes;

        public bool SyncTimeIntervalMinutesEnable => SyncMode is SyncMode.OnStaupAndTiming;

        /// <summary>
        /// WebDAV 服务地址
        /// </summary>
        [ObservableProperty]
        private string _syncApi;

        /// <summary>
        /// WebDAV 服务用户名
        /// </summary>
        [ObservableProperty]
        private string _syncUsername;

        /// <summary>
        /// WebDAV 服务密码
        /// </summary>
        [ObservableProperty]
        private string _syncPassword;
        #endregion

        #region 启动
        [ObservableProperty]
        private bool _startupOnLaunch;

        [ObservableProperty]
        private bool _slientStartup;
        #endregion

        #region 日志
        [ObservableProperty]
        private bool _logEnable;

        [ObservableProperty]
        private LogLevel _logMinLevel;

        [ObservableProperty]
        private string _logOutputFormat;

        [ObservableProperty]
        private string _logSizeContent;
        #endregion

        #region 更新
        [ObservableProperty]
        private DateTime _lastUpgradeCheckTime;

        [ObservableProperty]
        private bool _acceptBetaVersion;

        [ObservableProperty]
        private string _upgradeMirror;
        #endregion

        public AppSettingModel(
            AppSetting appSetting,
            ILogService logService,
            IDailySentenceService dailySentenceService
        )
        {
            _dailySentenceService = dailySentenceService;

            #region 外观
            _themeColor = appSetting.Appearance.ThemeColor;
            // _tintOpacity = appSetting.Appearance.TintOpacity;
            // _tintDarkColor = Color.Parse(appSetting.Appearance.TintDarkColor);
            // _tintLightColor = Color.Parse(appSetting.Appearance.TintLightColor);
            _isDailySentenceShown = appSetting.Appearance.SearchBar.IsDailySentenceEnabled;
            // _materialOpacity = appSetting.Appearance.MaterialOpacity;
            _officialWebsite = appSetting.Appearance.OfficialWebsite;
            //_isWindowPinned = appSetting.Appearance.IsWindowPinned;
            //_windowCloseBehavior = appSetting.Appearance.WindowCloseBehavior;

            #endregion

            #region 账号

            _autoLogin = appSetting.Account.AutoLogin;
            _rememberPassword = appSetting.Account.RememberPassword;

            #endregion

            #region 同步
            _syncMode = appSetting.Storage.Sync.Mode;
            _syncSecret = appSetting.Storage.Sync.Secret;
            _syncTimeIntervalMinutes = appSetting.Storage.Sync.TimeInterval.TotalMinutes;

            _syncApi = appSetting.Storage.Sync.Api;
            _syncUsername = appSetting.Storage.Sync.Username;
            _syncPassword = appSetting.Storage.Sync.Password;
            #endregion

            #region 启动
            _slientStartup = appSetting.Appearance.SearchBar.IsDailySentenceEnabled;
            _startupOnLaunch = appSetting.Appearance.SearchBar.IsDailySentenceEnabled;
            #endregion

            #region 日志
            _logEnable = appSetting.Services.Log.IsEnable;
            _logMinLevel = appSetting.Services.Log.MinLevel;
            _logOutputFormat = appSetting.Services.Log.OutputFormat;
            _logSizeContent =
                $"日志文件存储占用：{logService.LogDirectory.GetFileTotalSize($"*{AppDomain.CurrentDomain.FriendlyName}*.log").FormatFileSize()}";
            #endregion

            #region 更新
            _upgradeMirror = appSetting.Services.Upgrade.Mirror;
            _lastUpgradeCheckTime = appSetting.Services.Upgrade.LastCheckTime;
            _acceptBetaVersion = appSetting.Services.Upgrade.AcceptBetaVersion;
            #endregion

            // 保存配置至本地
            PropertyChanged += (sender, e) =>
            {
                #region 外观
                // appSetting.Appearance.ThemeMode = _themeColor;
                // appSetting.Apperance.TintOpacity = _tintOpacity;
                // appSetting.Apperance.TintDarkColor = _tintDarkColor.ToString();
                // appSetting.Apperance.TintLightColor = _tintLightColor.ToString();
                // appSetting.Apperance.MaterialOpacity = _materialOpacity;
                appSetting.Appearance.SearchBar.IsDailySentenceEnabled = _isDailySentenceShown;
                appSetting.Appearance.OfficialWebsite = _officialWebsite;
                //appSetting.Apperance.IsWindowPinned = _isWindowPinned;
                //appSetting.Apperance.WindowCloseBehavior = _windowCloseBehavior;
                #endregion

                #region 账号
                appSetting.Account.AutoLogin = _autoLogin;
                appSetting.Account.RememberPassword = _rememberPassword;
                #endregion

                #region 同步
                appSetting.Storage.Sync.Mode = _syncMode;
                appSetting.Storage.Sync.Secret = _syncSecret;
                appSetting.Storage.Sync.TimeInterval = TimeSpan.FromMinutes(_syncTimeIntervalMinutes);

                appSetting.Storage.Sync.Api = _syncApi;
                appSetting.Storage.Sync.Username = _syncUsername;
                appSetting.Storage.Sync.Password = _syncPassword;
                #endregion

                #region 启动
                appSetting.Appearance.SearchBar.IsDailySentenceEnabled = _slientStartup;
                appSetting.Appearance.SearchBar.IsDailySentenceEnabled = _startupOnLaunch;
                #endregion

                #region 日志
                if (
                    (appSetting.Services.Log.IsEnable != _logEnable)
                    || (appSetting.Services.Log.MinLevel != _logMinLevel)
                    || !appSetting.Services.Log.OutputFormat.Equals(_logOutputFormat)
                )
                {
                    appSetting.Services.Log.IsEnable = _logEnable;
                    appSetting.Services.Log.MinLevel = _logMinLevel;
                    appSetting.Services.Log.OutputFormat = _logOutputFormat;
                    logService.Setup(appSetting.Services.Log);
                }
                #endregion

                #region 更新
                appSetting.Services.Upgrade.Mirror = _upgradeMirror;
                appSetting.Services.Upgrade.LastCheckTime = _lastUpgradeCheckTime;
                appSetting.Services.Upgrade.AcceptBetaVersion = _acceptBetaVersion;
                #endregion
            };
        }

        public void NotifyTintColorChanged()
        {
            OnPropertyChanged(nameof(TintColor));
        }

        /// <summary>
        /// 去除每日一句末尾特殊字符
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"[.,;:，、。；‘“”：]+?$")]
        private static partial Regex DailySentenceRegex();
    }
}
