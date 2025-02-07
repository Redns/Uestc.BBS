//using System.Drawing;
//using CommunityToolkit.Mvvm.ComponentModel;
//using static System.Net.Mime.MediaTypeNames;

//namespace Uestc.BBS.Core.Models
//{
//    public partial class AppSettingModel(AppSetting setting) : ObservableObject
//    {
//        private readonly AppSetting _setting = setting;

//        #region 外观
//        /// <summary>
//        /// 主题色
//        /// </summary>
//        public ThemeColor ThemeColor
//        {
//            get => _setting.Apperance.ThemeMode;
//            set =>
//                SetProperty(
//                    _setting.Apperance.ThemeMode,
//                    value,
//                    _setting,
//                    (setting, color) => setting.Apperance.ThemeMode = color
//                );
//        }

//        /// <summary>
//        /// 颜色不透明度
//        /// </summary>
//        public double TintOpacity
//        {
//            get => _setting.Apperance.TintOpacity;
//            set =>
//                SetProperty(
//                    _setting.Apperance.TintOpacity,
//                    value,
//                    _setting,
//                    (setting, opacity) => setting.Apperance.TintOpacity = opacity
//                );
//        }

//        /// <summary>
//        /// 亚克力材质亮色
//        /// </summary>
//        [ObservableProperty]
//        [NotifyPropertyChangedFor(nameof(TintColor))]
//        private Color _tintLightColor;

//        /// <summary>
//        /// 亚克力主题暗色
//        /// </summary>
//        [ObservableProperty]
//        [NotifyPropertyChangedFor(nameof(TintColor))]
//        private Color _tintDarkColor;

//        /// <summary>
//        /// 亚克力材质颜色
//        /// </summary>
//        public Color TintColor =>
//            Application.Current!.ActualThemeVariant == ThemeVariant.Light
//                ? TintLightColor
//                : TintDarkColor;

//        /// <summary>
//        /// 材质不透明度
//        /// </summary>
//        [ObservableProperty]
//        private double _materialOpacity;

//        /// <summary>
//        /// 每日一句
//        /// </summary>
//        [ObservableProperty]
//        [NotifyPropertyChangedFor(nameof(DailySentence))]
//        private bool _isDailySentenceShown;

//        public Task<string> DailySentence =>
//            IsDailySentenceShown
//                ? _dailySentenceService
//                    .GetDailySentenceAsync()
//                    .ContinueWith(sentence =>
//                        DailySentenceRegex().Replace(sentence.Result, string.Empty)
//                    )
//                : Task.FromResult(string.Empty);

//        /// <summary>
//        /// 论坛官网
//        /// </summary>
//        [ObservableProperty]
//        private string _officialWebsite;

//        /// <summary>
//        /// 固定窗口
//        /// </summary>
//        [ObservableProperty]
//        private bool _isWindowPinned;

//        /// <summary>
//        /// 窗口关闭行为
//        /// </summary>
//        [ObservableProperty]
//        private WindowCloseBehavior _windowCloseBehavior;
//        #endregion

//        #region 账号
//        [ObservableProperty]
//        private bool _autoLogin;

//        [ObservableProperty]
//        private bool _rememberPassword;
//        #endregion

//        #region 同步
//        [ObservableProperty]
//        [NotifyPropertyChangedFor(nameof(SyncTimeIntervalMinutesEnable))]
//        private SyncMode _syncMode;

//        [ObservableProperty]
//        private string _syncSecret;

//        [ObservableProperty]
//        private double _syncTimeIntervalMinutes;

//        public bool SyncTimeIntervalMinutesEnable => SyncMode is SyncMode.OnStaupAndTiming;

//        /// <summary>
//        /// WebDAV 服务地址
//        /// </summary>
//        [ObservableProperty]
//        private string _syncApi;

//        /// <summary>
//        /// WebDAV 服务用户名
//        /// </summary>
//        [ObservableProperty]
//        private string _syncUsername;

//        /// <summary>
//        /// WebDAV 服务密码
//        /// </summary>
//        [ObservableProperty]
//        private string _syncPassword;
//        #endregion

//        #region 启动
//        [ObservableProperty]
//        private bool _startupOnLaunch;

//        [ObservableProperty]
//        private bool _slientStartup;
//        #endregion

//        #region 日志
//        [ObservableProperty]
//        private bool _logEnable;

//        [ObservableProperty]
//        private LogLevel _logMinLevel;

//        [ObservableProperty]
//        private string _logOutputFormat;

//        [ObservableProperty]
//        private string _logSizeContent;
//        #endregion

//        #region 更新
//        [ObservableProperty]
//        private DateTime _lastUpgradeCheckTime;

//        [ObservableProperty]
//        private bool _acceptBetaVersion;

//        [ObservableProperty]
//        private string _upgradeMirror;
//        #endregion
//    }
//}
