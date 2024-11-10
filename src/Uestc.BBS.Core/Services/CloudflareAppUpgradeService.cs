using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core.Helpers;

namespace Uestc.BBS.Core.Services
{
    public class CloudflareAppUpgradeService(string baseUrl, string secretId, string secretKey, string bucketName, string endpointUrl) : IAppUpgradeService
    {
        private readonly string _baseUrl = baseUrl;

        private readonly string _secretId = secretId;

        private readonly string _secretKey = secretKey;

        private readonly string _bucketName = bucketName;

        private readonly string _endpointUrl = endpointUrl;

        private readonly HttpClient _client = new();

        public async ValueTask<AppReleaseInfo> GetLatestRelease()
        {
            using var client = new AmazonS3Client(_secretId, _secretKey, new AmazonS3Config()
            {
                ServiceURL = _endpointUrl
            });

            // 获取最新版本号
            var operatingSystem = OperatingSystemHelper.GetOperatingSystem();
            var request = new ListObjectsRequest
            {
                BucketName = _bucketName,
                Prefix = operatingSystem
            };
            var response = await client.ListObjectsAsync(request);
            var latestReleaseVersion = response.S3Objects.Select(o => o.Key.Split('/')[1])
                                                         .OrderBy(v => int.Parse(v.Split(['.', '-'])[3]))
                                                         .Last();
            // 获取下载链接
            var systemArchitecture = OperatingSystemHelper.GetSystemArchitecture();
            var downloadUrl = response.S3Objects.FirstOrDefault(o =>
            {
                if (operatingSystem is "android" or "ios")
                {
                    return o.Key.Contains(latestReleaseVersion);
                }
                return o.Key.Contains(latestReleaseVersion) && o.Key.Contains(systemArchitecture);
            })?.Key;
            // 获取版本描述
            var description = await _client.GetStringAsync($"{_baseUrl}/{operatingSystem}/{latestReleaseVersion}/readme.md");

            return new AppReleaseInfo(latestReleaseVersion, description, $"{_baseUrl}/{downloadUrl}");
        }

        public async Task Upgrade(AppReleaseInfo info)
        {
            var currentAppVersion = OperatingSystemHelper.GetAppVersion();
            if (currentAppVersion?.Revision >= info.Version.Revision)
            {
                return;
            }

            // 下载安装包
            var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), info.DownloadName);
            using var downloadWriteStream = File.OpenWrite(downloadPath);
            using var downloadReadStream = await _client.GetStreamAsync(info.DownloadUrl);
            await downloadReadStream.CopyToAsync(downloadWriteStream);

            // TODO 执行安装

        }
    }
}