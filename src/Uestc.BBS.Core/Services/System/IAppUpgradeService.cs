namespace Uestc.BBS.Core.Services.System
{
    public interface IAppUpgradeService
    {
        ValueTask<AppReleaseInfo> GetLatestRelease(CancellationToken token = default);

        Task Upgrade(AppReleaseInfo info, CancellationToken cancellationToken = default);
    }

    public class AppReleaseInfo(string versionName, string description, string downloadUrl)
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public readonly Version Version = new(versionName.Split('-')[0]);

        /// <summary>
        /// 是否为 Beta 版本
        /// </summary>
        public readonly bool IsBetaVersion = versionName.Contains(
            "beta",
            StringComparison.CurrentCultureIgnoreCase
        );

        /// <summary>
        /// 版本名称
        /// 版本命名规则：<AppName>-x.x.x.x-<beta>-<rid>.<ext>
        /// 如：
        ///     1. Uestc.BBS.Desktop-0.0.1.113-beta-x64.exe
        ///     2. Uestc.BBS.Desktop-0.0.1.113-beta-arm.deb
        ///     3. Uestc.BBS.Desktop-0.0.1.113-beta.apk
        /// </summary>
        public readonly string VersionName = versionName;

        /// <summary>
        /// 版本描述
        /// </summary>
        public readonly string Description = description;

        /// <summary>
        /// 下载文件名称
        /// </summary>
        public readonly string DownloadName = downloadUrl.Split('/').Last();

        /// <summary>
        /// 下载链接
        /// </summary>
        public readonly string DownloadUrl = downloadUrl;
    }
}
