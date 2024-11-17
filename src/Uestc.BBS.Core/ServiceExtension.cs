using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services;

namespace Uestc.BBS.Core
{
    public static class ServiceExtension
    {
        public static readonly ServiceCollection ServiceCollection = new();

        private static ServiceProvider? _services;
        public static ServiceProvider Services
        {
            get
            {
                return _services ??= ServiceCollection.BuildServiceProvider();
            }
        }

        /// <summary>
        /// 初始化服务（全局服务）
        /// </summary>
        /// <param name="collection">服务集合</param>
        /// <returns></returns>
        public static ServiceCollection ConfigureCommonServices()
        {
            // AppSetting
            ServiceCollection.AddSingleton(settings =>
            {
                var appName = AppDomain.CurrentDomain.FriendlyName;
                var deviceId = Services.GetRequiredService<string>();
                var appSecretPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, "secret.aes");
                var appSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, "appsettings.aes");
                return AppSetting.Load(appSettingsPath, File.Exists(appSecretPath) ? File.ReadAllText(appSecretPath).Decrypt(deviceId) : string.Empty);
            });
            // HttpClient
            ServiceCollection.AddHttpClient();
            // App upgrade
            ServiceCollection.AddSingleton<IAppUpgradeService>(appUpgrade => new CloudflareAppUpgradeService("https://distributor.krins.cloud",
                "679edd7cffaf4a9ef3be4c445317a461",
                "45fcd2ff239321d48ad4cae7ea9b5c4457f9d12f2483eb4029836931f5f83526",
                "distributor",
                "https://11f33fc072df859ebaf8faa0b0e1766b.r2.cloudflarestorage.com"));

            return ServiceCollection;
        }
    }
}
