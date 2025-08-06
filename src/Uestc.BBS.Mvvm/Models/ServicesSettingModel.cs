using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Mvvm.Models
{
    public class ServicesSettingModel(ServicesSetting setting) : ObservableObject
    {
        public LogSettingModel Log { get; init; } = new(setting.Log);

        public NetworkSettingModel Network { get; init; } = new(setting.Network);

        public UpgradeSettingModel Upgrade { get; init; } = new(setting.Upgrade);

        public StartupAndShutdownSettingModel StartupAndShutdown { get; init; } =
            new(setting.StartupAndShutdown);
    }

    public class LogSettingModel(LogSetting logSetting) : ObservableObject
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool IsEnable
        {
            get => logSetting.IsEnable;
            set => SetProperty(logSetting.IsEnable, value, logSetting, (s, e) => s.IsEnable = e);
        }

        /// <summary>
        /// 最低日志级别
        /// </summary>
        public LogLevel MinLevel
        {
            get => logSetting.MinLevel;
            set => SetProperty(logSetting.MinLevel, value, logSetting, (s, e) => s.MinLevel = e);
        }

        /// <summary>
        /// 输出格式
        /// </summary>
        public string OutputFormat
        {
            get => logSetting.OutputFormat;
            set =>
                SetProperty(
                    logSetting.OutputFormat,
                    value,
                    logSetting,
                    (s, e) => s.OutputFormat = e
                );
        }

        /// <summary>
        /// 日志存档大小
        /// </summary>
        public long ArchiveAboveSize
        {
            get => logSetting.ArchiveAboveSize;
            set =>
                SetProperty(
                    logSetting.ArchiveAboveSize,
                    value,
                    logSetting,
                    (s, e) => s.ArchiveAboveSize = e
                );
        }

        /// <summary>
        /// 最大日志文件数
        /// </summary>
        public int MaxArchiveFiles
        {
            get => logSetting.MaxArchiveFiles;
            set =>
                SetProperty(
                    logSetting.MaxArchiveFiles,
                    value,
                    logSetting,
                    (s, e) => s.MaxArchiveFiles = e
                );
        }
    }

    public class NetworkSettingModel(NetworkSetting setting) : ObservableObject
    {
        public bool IsSystermProxyEnabled
        {
            get => setting.IsSystemProxyEnabled;
            set =>
                SetProperty(
                    setting.IsSystemProxyEnabled,
                    value,
                    setting,
                    (s, e) => s.IsSystemProxyEnabled = e
                );
        }

        /// <summary>
        /// 是否启用证书验证
        /// </summary>
        public bool IsCertificateVerificationEnabled
        {
            get => setting.IsCertificateVerificationEnabled;
            set =>
                SetProperty(
                    setting.IsCertificateVerificationEnabled,
                    value,
                    setting,
                    (s, e) => s.IsCertificateVerificationEnabled = e
                );
        }

        /// <summary>
        /// 基地址
        /// </summary>
        public string BaseUrl
        {
            get => setting.BaseUrl;
            set => SetProperty(setting.BaseUrl, value, setting, (s, e) => s.BaseUrl = e);
        }

        /// <summary>
        /// 每日一句
        /// </summary>
        public string DailySentenceUrl
        {
            get => setting.DailySentenceUrl;
            set =>
                SetProperty(
                    setting.DailySentenceUrl,
                    value,
                    setting,
                    (s, e) => s.DailySentenceUrl = e
                );
        }

        /// <summary>
        /// 授权
        /// </summary>
        public string AuthUrl
        {
            get => setting.AuthUrl;
            set => SetProperty(setting.AuthUrl, value, setting, (s, e) => s.AuthUrl = e);
        }

        /// <summary>
        /// 主题详情
        /// </summary>
        public string TopicDetailUrl
        {
            get => setting.TopicDetailUrl;
            set =>
                SetProperty(setting.TopicDetailUrl, value, setting, (s, e) => s.TopicDetailUrl = e);
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        public string TopicListUrl
        {
            get => setting.TopicListUrl;
            set => SetProperty(setting.TopicListUrl, value, setting, (s, e) => s.TopicListUrl = e);
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        public string UserDetailUrl
        {
            get => setting.UserDetailUrl;
            set =>
                SetProperty(setting.UserDetailUrl, value, setting, (s, e) => s.UserDetailUrl = e);
        }
    }

    public class UpgradeSettingModel(UpgradeSetting upgradeSetting) : ObservableObject
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool AcceptBetaVersion
        {
            get => upgradeSetting.AcceptBetaVersion;
            set =>
                SetProperty(
                    upgradeSetting.AcceptBetaVersion,
                    value,
                    upgradeSetting,
                    (s, e) => s.AcceptBetaVersion = e
                );
        }

        /// <summary>
        /// 上次更新检查时间
        /// </summary>
        public DateTime LastCheckTime
        {
            get => upgradeSetting.LastCheckTime;
            set =>
                SetProperty(
                    upgradeSetting.LastCheckTime,
                    value,
                    upgradeSetting,
                    (s, e) => s.LastCheckTime = e
                );
        }

        /// <summary>
        /// 更新地址
        /// </summary>
        public string Mirror
        {
            get => upgradeSetting.Mirror;
            set =>
                SetProperty(upgradeSetting.Mirror, value, upgradeSetting, (s, e) => s.Mirror = e);
        }
    }

    /// <summary>
    /// 启动和关闭设置
    /// </summary>
    public class StartupAndShutdownSettingModel(StartupAndShutdownSetting startupAndShutdownSetting)
        : ObservableObject
    {
        /// <summary>
        /// 静默启动
        /// </summary>
        public bool SilentStart
        {
            get => startupAndShutdownSetting.SilentStart;
            set =>
                SetProperty(
                    startupAndShutdownSetting.SilentStart,
                    value,
                    startupAndShutdownSetting,
                    (s, e) => s.SilentStart = e
                );
        }

        /// <summary>
        /// 固定窗口
        /// </summary>
        public bool IsWindowPinned
        {
            get => startupAndShutdownSetting.IsWindowPinned;
            set =>
                SetProperty(
                    startupAndShutdownSetting.IsWindowPinned,
                    value,
                    startupAndShutdownSetting,
                    (s, e) => s.IsWindowPinned = e
                );
        }

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        public WindowCloseBehavior WindowCloseBehavior
        {
            get => startupAndShutdownSetting.WindowCloseBehavior;
            set =>
                SetProperty(
                    startupAndShutdownSetting.WindowCloseBehavior,
                    value,
                    startupAndShutdownSetting,
                    (s, e) => s.WindowCloseBehavior = e
                );
        }
    }
}
