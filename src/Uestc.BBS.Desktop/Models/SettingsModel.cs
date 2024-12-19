using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Desktop.Models
{
    public partial class SettingsModel : ObservableObject
    {
        #region 外观
        [ObservableProperty]
        private ThemeColor _themeColor;

        [ObservableProperty]
        private string _officialWebsite;
        #endregion

        #region 账号
        [ObservableProperty]
        private bool _autoLogin;

        [ObservableProperty]
        private bool _rememberPassword;
        #endregion

        #region 同步
        [ObservableProperty]
        private SyncMode _syncMode;

        [ObservableProperty]
        private string _syncSecret;

        [ObservableProperty]
        private double _syncTimeIntervalMinutes;

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
        #endregion

        public SettingsModel(AppSetting appSetting, ILogService logService)
        {
            #region 外观
            _themeColor = appSetting.Apperance.ThemeColor;
            _officialWebsite = appSetting.Apperance.OfficialWebsite;
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
            #endregion

            // 保存配置至本地
            PropertyChanged += (sender, e) =>
            {
                #region 外观
                appSetting.Apperance.ThemeColor = _themeColor;
                appSetting.Apperance.OfficialWebsite = _officialWebsite;
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
                if ((appSetting.Log.IsEnable != _logEnable) || (appSetting.Log.MinLevel != _logMinLevel) || !appSetting.Log.OutputFormat.Equals(_logOutputFormat))
                {
                    logService.Setup(appSetting.Log);
                }

                appSetting.Log.IsEnable = _logEnable;
                appSetting.Log.MinLevel = _logMinLevel;
                appSetting.Log.OutputFormat = _logOutputFormat;
                #endregion

                appSetting.Save();
            };
        }
    }
}
