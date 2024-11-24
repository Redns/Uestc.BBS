using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Core.Services.Api.User;
using Uestc.BBS.Core.Services.System;

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
                var appSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, "appsettings.json");
                return AppSetting.Load(appSettingsPath);
            });
            // Forums
            ServiceCollection.AddTransient<IAuthService, AuthService>();
            ServiceCollection.AddTransient<ITopicService, TopicService>();
            // HttpClient
            ServiceCollection.AddHttpClient();
            ServiceCollection.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://bbs.uestc.edu.cn/mobcent/app/web/index.php?r=user/login");
            });
            ServiceCollection.AddHttpClient<ITopicService, TopicService>((services, client) =>
            {
                var appSetting = services.GetService<AppSetting>();
                var credential = appSetting?.Auth.DefaultCredential;
                client.BaseAddress = new Uri($"https://bbs.uestc.edu.cn/mobcent/app/web/index.php?r=forum/topiclist&accessToken={credential?.Token}&accessSecret={credential?.Secret}");
            });
            ServiceCollection.AddHttpClient<IUserService, UserService>((services, client) =>
            {
                var appSetting = services.GetService<AppSetting>();
                var credential = appSetting?.Auth.DefaultCredential;
                client.BaseAddress = new Uri($"https://bbs.uestc.edu.cn/mobcent/app/web/index.php?accessToken={credential?.Token}&accessSecret={credential?.Secret}");
            });
           
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
