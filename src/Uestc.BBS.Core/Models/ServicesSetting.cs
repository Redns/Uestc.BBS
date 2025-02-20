using FastEnumUtility;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Core.Models
{
    public class ServicesSetting
    {
        /// <summary>
        /// 日志设置
        /// </summary>
        public LogSetting Log { get; set; } = new();

        /// <summary>
        /// 更新设置
        /// </summary>
        public UpgradeSetting Upgrade { get; set; } = new();

        /// <summary>
        /// 启动和关闭设置
        /// </summary>
        public StartupAndShutdownSetting StartupAndShutdown { get; set; } = new();
    }

    /// <summary>
    /// 日志设置
    /// </summary>
    public class LogSetting
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// 最低日志级别
        /// </summary>
        public LogLevel MinLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// 输出格式
        /// </summary>
        public string OutputFormat { get; set; } =
            "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [Uestc.BBS ${level}] ${message}${onexception:${newline}${exception:format=toString}${exception:format=StackTrace}}";

        /// <summary>
        /// 日志存档大小（MB）
        /// </summary>
        public long ArchiveAboveSize { get; set; } = 8;

        /// <summary>
        /// 最大日志文件数
        /// </summary>
        public int MaxArchiveFiles { get; set; } = 8;
    }

    /// <summary>
    /// 更新设置
    /// </summary>
    public class UpgradeSetting
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool AcceptBetaVersion { get; set; } = false;

        /// <summary>
        /// 上次更新检查时间
        /// </summary>
        public DateTime LastCheckTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 更新地址
        /// </summary>
        public string Mirror { get; set; } = "https://mirrors.krins.cloud/Uestc.BBS";
    }

    /// <summary>
    /// 启动和关闭设置
    /// </summary>
    public class StartupAndShutdownSetting
    {
        /// <summary>
        /// 静默启动
        /// </summary>
        public bool SilentStart { get; set; } = false;

        /// <summary>
        /// 固定窗口
        /// </summary>
        public bool IsWindowPinned { get; set; } = false;

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        public WindowCloseBehavior WindowCloseBehavior { get; set; } = WindowCloseBehavior.Hide;
    }

    /// <summary>
    /// 窗口关闭行为
    /// </summary>
    public enum WindowCloseBehavior
    {
        [Label("退出应用")]
        Exit,

        [Label("隐藏窗口")]
        Hide,

        [Label("隐藏窗口 + 效率模式")]
        HideWithEfficiencyMode,
    }
}
