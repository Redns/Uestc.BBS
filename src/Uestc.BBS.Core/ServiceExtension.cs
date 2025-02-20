using Microsoft.Extensions.DependencyInjection;
using NLog;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.Auth;
using Uestc.BBS.Core.Services.Forum.TopicList;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Core.Services.User;

namespace Uestc.BBS.Core
{
    public static class ServiceExtension
    {
        public static readonly ServiceCollection ServiceCollection = new();

        private static ServiceProvider? _services;
        public static ServiceProvider Services
        {
            get { return _services ??= ServiceCollection.BuildServiceProvider(); }
        }

        /// <summary>
        /// 初始化服务（全局服务）
        /// </summary>
        /// <param name="collection">服务集合</param>
        /// <returns></returns>
        public static ServiceCollection ConfigureCommonServices()
        {
            ServiceCollection
                // AppSetting
                .AddSingleton(AppSetting.Load())
                // 日志
                .AddSingleton<ILogService>(services =>
                {
                    var nlogger = new NLogService(LogManager.GetLogger("*"));
                    var appSetting = services.GetRequiredService<AppSetting>();
                    nlogger.Setup(appSetting.Services.Log);
                    return nlogger;
                })
                // Forums
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<ITopicListService, TopicListService>()
                // 每日一句
                .AddSingleton<IDailySentenceService, DailySentenceService>();
            // HttpClient
            ServiceCollection.AddHttpClient();
            ServiceCollection.AddHttpClient<IDailySentenceService, DailySentenceService>(client =>
            {
                client.BaseAddress = new Uri("https://bbs.uestc.edu.cn/forum.php?mobile=no");
            });
            ServiceCollection.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri(
                    "https://bbs.uestc.edu.cn/mobcent/app/web/index.php?r=user/login"
                );
            });
            ServiceCollection.AddHttpClient<ITopicListService, TopicListService>(
                (services, client) =>
                {
                    var appSetting = services.GetService<AppSetting>();
                    var credential = appSetting?.Account.DefaultCredential;
                    client.BaseAddress = new Uri(
                        $"https://bbs.uestc.edu.cn/mobcent/app/web/index.php?accessToken={credential?.Token}&accessSecret={credential?.Secret}"
                    );
                }
            );
            ServiceCollection.AddHttpClient<IUserService, UserService>(
                (services, client) =>
                {
                    var appSetting = services.GetService<AppSetting>();
                    var credential = appSetting?.Account.DefaultCredential;
                    client.BaseAddress = new Uri(
                        $"https://bbs.uestc.edu.cn/mobcent/app/web/index.php?accessToken={credential?.Token}&accessSecret={credential?.Secret}"
                    );
                }
            );

            return ServiceCollection;
        }
    }
}
