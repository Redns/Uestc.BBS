namespace Uestc.BBS.Core.Services.System
{
    /// <summary>
    /// 开机自启动
    /// </summary>
    public interface IStartupService
    {
        bool Enable();

        bool Disable();
    }

    /// <summary>
    /// 自启动信息
    /// </summary>
    public class StartupInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApplicationName { get; set; } = string.Empty;

        /// <summary>
        /// 工作路径
        /// </summary>
        public string WorkingDirectory { get; set; } = string.Empty;

        /// <summary>
        /// 应用路径
        /// </summary>
        public string ApplicationPath { get; set; } = string.Empty;
    }
}
