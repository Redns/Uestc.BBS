using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core.Services;

namespace Uestc.BBS.Core
{
    public class ServiceExtension
    {
        private static readonly ServiceCollection _serviceCollection = new();

        private static ServiceProvider? _services;

        /// <summary>
        /// 初始化服务（全局服务）
        /// </summary>
        /// <param name="collection">服务集合</param>
        /// <returns></returns>
        public static ServiceProvider ConfigureServices(Action<ServiceCollection> configurePlatformServices)
        {
            // HttpClient
            _serviceCollection.AddHttpClient();
            // App upgrade
            _serviceCollection.AddSingleton<IAppUpgradeService>(appUpgrade => new CloudflareAppUpgradeService("https://distributor.krins.cloud",
                "679edd7cffaf4a9ef3be4c445317a461",
                "45fcd2ff239321d48ad4cae7ea9b5c4457f9d12f2483eb4029836931f5f83526",
                "distributor",
                "https://11f33fc072df859ebaf8faa0b0e1766b.r2.cloudflarestorage.com"));
            // 初始化平台特定服务
            configurePlatformServices(_serviceCollection);
            
            return _services = _serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// 获取服务，若服务不存在则返回 null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T? GetService<T>()
        {
            return (T?)_services!.GetService(typeof(T));
        }

        /// <summary>
        /// 获取服务，若服务不存在则抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRequiredService<T>() where T : notnull
        {
            return (T)_services!.GetRequiredService(typeof(T));
        }
    }
}
