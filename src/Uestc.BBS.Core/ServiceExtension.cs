using System.Net;
using System.Net.Security;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Polly;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Auth;

namespace Uestc.BBS.Core
{
    public static class ServiceExtension
    {
        public static readonly ServiceCollection Container = new();

        private static ServiceProvider? _services;
        public static ServiceProvider Services
        {
            get { return _services ??= Container.BuildServiceProvider(); }
        }

        /// <summary>
        /// 初始化服务（全局服务）
        /// </summary>
        /// <param name="collection">服务集合</param>
        /// <returns></returns>
        public static ServiceCollection ConfigureCommonServices()
        {
            Container
                // AppSetting
                .AddSingleton(AppSetting.Load())
                // API 基地址
                .AddSingleton(services =>
                    services.GetRequiredService<AppSetting>().Services.Network.BaseUri
                )
                // 日志
                .AddSingleton<ILogService>(services =>
                {
                    var nlogger = new NLogService(LogManager.GetLogger("*"));
                    var appSetting = services.GetRequiredService<AppSetting>();
                    nlogger.Setup(appSetting.Services.Log);
                    return nlogger;
                })
                // HttpClient
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
            // Web HttpClient
            Container
                .UseWebServices(
                    services =>
                        services
                            .GetRequiredService<AppSetting>()
                            .Account.DefaultCredential?.CookieContainer,
                    services =>
                        services
                            .GetRequiredService<AppSetting>()
                            .Account.DefaultCredential?.Authorization ?? string.Empty,
                    services => services.GetRequiredService<Uri>()
                )
                .AddPolicyHandler(
                    (services, request) =>
                    {
                        var appSetting = Services.GetRequiredService<AppSetting>();
                        var authServices = Services.GetRequiredKeyedService<IAuthService>(
                            ServiceExtensions.WEB_API
                        );

                        return Policy<HttpResponseMessage>
                            .HandleResult(r => r.StatusCode == HttpStatusCode.Unauthorized)
                            .RetryAsync(
                                3,
                                async (_, _) =>
                                {
                                    var authCredential = appSetting.Account.DefaultCredential;
                                    if (authCredential is null)
                                    {
                                        return;
                                    }

                                    // 获取 Cookie & Authorization
                                    await authServices.LoginAsync(authCredential);

                                    // XXX 重新设置 Authorization
                                    // 由于 Authorization 为 string 类型，所以需要移除原有的 Authorization 并重新设置，CookieContainer 则不需要
                                    // 此处如果不移除旧的 Authorization，会导致多个 Authorization 以逗号分隔连接在一起，导致请求失败
                                    request.Headers.Remove("Authorization");
                                    request.Headers.TryAddWithoutValidation(
                                        "Authorization",
                                        authCredential.Authorization
                                    );
                                }
                            );
                    }
                );
            // Mobcent HttpClient
            // TODO 授权失效时使用 polly 重新获取，注意未登录时 API 返回状态码 200，需要根据 Json 解析后 Header 中的 ErrorCode 判断是否需要重新登录
            Container.UseMobcentServices(
                services =>
                {
                    var appSetting = services.GetRequiredService<AppSetting>();
                    return (
                        appSetting.Account.DefaultCredential?.Token ?? string.Empty,
                        appSetting.Account.DefaultCredential?.Secret ?? string.Empty
                    );
                },
                services => services.GetRequiredService<Uri>()
            );
            // Web & Mobcent
            Container
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
                .AddWebThreadReplyService()
                // 好友列表
                .AddFriendListService()
                // 公告
                .AddWebAnnouncementService()
                // 全站信息
                .AddWebGlobalStatusService();

            return Container;
        }
    }
}
