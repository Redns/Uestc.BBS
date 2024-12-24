using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Desktop.Models
{
    public partial class AppSettingModel : ObservableObject
    {
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
        /// 论坛官网
        /// </summary>
        [ObservableProperty]
        private string _officialWebsite;

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

        public AppSettingModel(AppSetting appSetting, ILogService logService)
        {
            #region 外观
            _themeColor = appSetting.Apperance.ThemeMode;
            _tintOpacity = appSetting.Apperance.TintOpacity;
            _tintDarkColor = Color.Parse(appSetting.Apperance.TintDarkColor);
            _tintLightColor = Color.Parse(appSetting.Apperance.TintLightColor);
            _materialOpacity = appSetting.Apperance.MaterialOpacity;
            _officialWebsite = appSetting.Apperance.OfficialWebsite;
            _windowCloseBehavior = appSetting.Apperance.WindowCloseBehavior;

            #endregion

            #region 账号

            _autoLogin = appSetting.Auth.AutoLogin;
            _rememberPassword = appSetting.Auth.RememberPassword;

            #endregion

            #region 同步
            _syncMode = appSetting.Sync.Mode;
            _syncSecret = appSetting.Sync.Secret;
            _syncTimeIntervalMinutes = appSetting.Sync.TimeInterval.TotalMinutes;

            _syncApi = appSetting.Sync.Api;
            _syncUsername = appSetting.Sync.Username;
            _syncPassword = appSetting.Sync.Password;
            #endregion

            #region 启动
            _slientStartup = appSetting.Apperance.SlientStart;
            _startupOnLaunch = appSetting.Apperance.StartupOnLaunch;
            #endregion

            #region 日志
            _logEnable = appSetting.Log.IsEnable;
            _logMinLevel = appSetting.Log.MinLevel;
            _logOutputFormat = appSetting.Log.OutputFormat;
            _logSizeContent =
                $"日志文件存储占用：{logService.LogDirectory.GetFileTotalSize($"*{AppDomain.CurrentDomain.FriendlyName}*.log").FormatFileSize()}";
            #endregion

            #region 更新
            _upgradeMirror = appSetting.Upgrade.Mirror;
            _lastUpgradeCheckTime = appSetting.Upgrade.LastCheckTime;
            _acceptBetaVersion = appSetting.Upgrade.AcceptBetaVersion;
            #endregion

            // 保存配置至本地
            PropertyChanged += (sender, e) =>
            {
                #region 外观
                appSetting.Apperance.ThemeMode = _themeColor;
                appSetting.Apperance.TintOpacity = _tintOpacity;
                appSetting.Apperance.TintDarkColor = _tintDarkColor.ToString();
                appSetting.Apperance.TintLightColor = _tintLightColor.ToString();
                appSetting.Apperance.MaterialOpacity = _materialOpacity;
                appSetting.Apperance.OfficialWebsite = _officialWebsite;
                appSetting.Apperance.WindowCloseBehavior = _windowCloseBehavior;
                #endregion

                #region 账号
                appSetting.Auth.AutoLogin = _autoLogin;
                appSetting.Auth.RememberPassword = _rememberPassword;
                #endregion

                #region 同步
                appSetting.Sync.Mode = _syncMode;
                appSetting.Sync.Secret = _syncSecret;
                appSetting.Sync.TimeInterval = TimeSpan.FromMinutes(_syncTimeIntervalMinutes);

                appSetting.Sync.Api = _syncApi;
                appSetting.Sync.Username = _syncUsername;
                appSetting.Sync.Password = _syncPassword;
                #endregion

                #region 启动
                appSetting.Apperance.SlientStart = _slientStartup;
                appSetting.Apperance.StartupOnLaunch = _startupOnLaunch;
                #endregion

                #region 日志
                if (
                    (appSetting.Log.IsEnable != _logEnable)
                    || (appSetting.Log.MinLevel != _logMinLevel)
                    || !appSetting.Log.OutputFormat.Equals(_logOutputFormat)
                )
                {
                    appSetting.Log.IsEnable = _logEnable;
                    appSetting.Log.MinLevel = _logMinLevel;
                    appSetting.Log.OutputFormat = _logOutputFormat;
                    logService.Setup(appSetting.Log);
                }
                #endregion

                #region 更新
                appSetting.Upgrade.Mirror = _upgradeMirror;
                appSetting.Upgrade.LastCheckTime = _lastUpgradeCheckTime;
                appSetting.Upgrade.AcceptBetaVersion = _acceptBetaVersion;
                #endregion
            };
        }

        public void NotifyTintColorChanged()
        {
            OnPropertyChanged(nameof(TintColor));
        }
    }
}
