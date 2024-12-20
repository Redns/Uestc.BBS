using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Core.Services.Api.User;

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
            ServiceCollection.AddSingleton(settings => AppSetting.Load());
            // Forums
            ServiceCollection.AddTransient<IAuthService, AuthService>();
            ServiceCollection.AddTransient<ITopicService, TopicService>();
            //  Github REST
            ServiceCollection.AddTransient<IGithubRESTService, GithubRESTService>();
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
            ServiceCollection.AddHttpClient<IGithubRESTService, GithubRESTService>((services, client) =>
            {
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.Add("User-Agent", "YourApp");
            });

            return ServiceCollection;
        }
    }
}
