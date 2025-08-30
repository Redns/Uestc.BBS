using System.Net;
using System.Net.Security;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Sdk;

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
                // AuthCredential
                .AddSingleton(services =>
                    services.GetRequiredService<AppSetting>().Account.DefaultCredential
                    ?? throw new ArgumentNullException(nameof(AppSetting.Account.DefaultCredential))
                )
                // 日志
                .AddSingleton<ILogService>(services =>
                {
                    var nlogger = new NLogService(LogManager.GetLogger("*"));
                    var appSetting = services.GetRequiredService<AppSetting>();
                    nlogger.Setup(appSetting.Services.Log);
                    return nlogger;
                });
            // HttpClient
            ServiceCollection
                .AddHttpClient()
                .ConfigureHttpClientDefaults(builder =>
                    builder.ConfigurePrimaryHttpMessageHandler(
                        (handler, services) =>
                        {
                            if (handler is not SocketsHttpHandler socketsHttpHandler)
                            {
                                return;
                            }

                            var appSetting = services.GetRequiredService<AppSetting>();

                            // 系统代理 & SSL
                            socketsHttpHandler.UseProxy = appSetting
                                .Services
                                .Network
                                .UseSystemProxy;
                            socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (
                                sender,
                                cert,
                                chain,
                                sslPolicyErrors
                            ) =>
                            {
                                return appSetting.Services.Network.IsCertificateVerificationEnabled
                                    || sslPolicyErrors == SslPolicyErrors.None;
                            };

                            // 多连接 & 压缩
                            socketsHttpHandler.EnableMultipleHttp2Connections = true;
                            socketsHttpHandler.AutomaticDecompression = DecompressionMethods.All;
                        }
                    )
                );

            ServiceCollection.UseWebServices(
                services =>
                    services
                        .GetRequiredService<AppSetting>()
                        .Account.DefaultCredential?.CookieContainer,
                services =>
                    services
                        .GetRequiredService<AppSetting>()
                        .Account.DefaultCredential?.Authorization ?? string.Empty,
                services => services.GetRequiredService<AppSetting>().Services.Network.BaseUri
            );

            ServiceCollection.UseMobcentServices(services =>
                services.GetRequiredService<AppSetting>().Services.Network.BaseUri
            );

            ServiceCollection
                // 登录授权
                .AddWebAuthService()
                .AddMobcentAuthService()
                // 每日一句
                .AddDailySentencesService()
                // 主题列表
                .AddMobcentThreadListService()
                // 主题内容 & 评论
                .AddMobcentThreadContentService()
                // 回复
                .AddWebThreadReplyService();

            return ServiceCollection;
        }
    }
}
