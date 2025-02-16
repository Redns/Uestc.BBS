using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Mvvm.Models
{
    public class ServicesSettingModel(ServicesSetting setting) : ObservableObject
    {
        public LogSettingModel Log { get; init; } = new(setting.Log);

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
            set =>
                SetProperty(
                    logSetting.IsEnable,
                    value,
                    logSetting,
                    (setting, isEnabled) => setting.IsEnable = isEnabled
                );
        }

        /// <summary>
        /// 最低日志级别
        /// </summary>
        public LogLevel MinLevel
        {
            get => logSetting.MinLevel;
            set =>
                SetProperty(
                    logSetting.MinLevel,
                    value,
                    logSetting,
                    (setting, minLevel) => logSetting.MinLevel = minLevel
                );
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
                    (logSetting, format) => logSetting.OutputFormat = format
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
                    (setting, size) => setting.ArchiveAboveSize = size
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
                    (setting, count) => setting.MaxArchiveFiles = count
                );
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
                    (setting, accept) => setting.AcceptBetaVersion = accept
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
                    (setting, time) => setting.LastCheckTime = time
                );
        }

        /// <summary>
        /// 更新地址
        /// </summary>
        public string Mirror
        {
            get => upgradeSetting.Mirror;
            set =>
                SetProperty(
                    upgradeSetting.Mirror,
                    value,
                    upgradeSetting,
                    (setting, mirror) => setting.Mirror = mirror
                );
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
                    (setting, enabled) => setting.SilentStart = enabled
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
                    (setting, pinned) => setting.IsWindowPinned = pinned
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
                    (setting, behavior) => setting.WindowCloseBehavior = behavior
                );
        }
    }
}
