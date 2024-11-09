using System;
using System.Threading.Tasks;

namespace Uestc.BBS.Services
{
    public interface IAppUpgradeService
    {
        ValueTask<AppReleaseInfo> GetLatestRelease();

        Task Upgrade(AppReleaseInfo info);
    }

    public class AppReleaseInfo (string versionName, string description, string downloadUrl)
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public Version Version { get; set; } = new Version(versionName.Split('-')[0]);

        /// <summary>
        /// 是否为 Beta 版本
        /// </summary>
        public bool IsBetaVersion { get; set; } = versionName.Contains("beta", StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// 版本名称
        /// 版本命名规则：<AppName>-x.x.x.x-<beta>-<rid>.<ext>
        /// 如：
        ///     1. Uestc.BBS.Desktop-0.0.1.113-beta-x64.exe
        ///     2. Uestc.BBS.Desktop-0.0.1.113-beta-arm.deb
        ///     3. Uestc.BBS.Desktop-0.0.1.113-beta.apk
        /// </summary>
        public string VersionName { get; set; } = versionName;

        /// <summary>
        /// 版本描述
        /// </summary>
        public string Description { get; set; } = description;

        /// <summary>
        /// 下载链接
        /// </summary>
        public string DownloadUrl { get; set; } = downloadUrl;
    }   
}