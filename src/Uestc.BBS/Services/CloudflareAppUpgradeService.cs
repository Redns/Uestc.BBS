namespace Uestc.BBS.Services
{
    public class CloudflareAppUpgradeService : IAppUpgradeService
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ValueTask<AppReleaseInfo> GetLatestRelease()
        {
            return Task.FromResult(0);
        }

        public Task Upgrade(AppReleaseInfo info)
        {
            return Task.FromResult();
        }
    }
}