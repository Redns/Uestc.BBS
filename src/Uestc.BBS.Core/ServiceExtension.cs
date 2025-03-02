using System.Net.Security;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.Auth;
using Uestc.BBS.Core.Services.Forum;
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
                })
                // Forums
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<ITopicService, TopicService>()
                .AddTransient<ITopicListService, TopicListService>()
                // 每日一句
                .AddSingleton<IDailySentenceService, DailySentenceService>();
            // HttpClient
            ServiceCollection.AddHttpClient();
            ServiceCollection
                .AddHttpClient<IDailySentenceService, DailySentenceService>(
                    (services, client) =>
                    {
                        client.BaseAddress = services
                            .GetRequiredService<AppSetting>()
                            .Api.DailySentenceUri;
                    }
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
                            socketsHttpHandler.UseProxy = appSetting.Api.IsSystemProxyEnabled;
                            socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (
                                sender,
                                cert,
                                chain,
                                sslPolicyErrors
                            ) =>
                            {
                                return appSetting.Api.IsCertificateVerificationEnabled
                                    || sslPolicyErrors == SslPolicyErrors.None;
                            };
                        }
                    }
                );
            ServiceCollection
                .AddHttpClient<IAuthService, AuthService>(
                    (services, client) =>
                    {
                        client.BaseAddress = services.GetRequiredService<AppSetting>().Api.AuthUri;
                    }
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
                            socketsHttpHandler.UseProxy = appSetting.Api.IsSystemProxyEnabled;
                            socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (
                                sender,
                                cert,
                                chain,
                                sslPolicyErrors
                            ) =>
                            {
                                return appSetting.Api.IsCertificateVerificationEnabled
                                    || sslPolicyErrors == SslPolicyErrors.None;
                            };
                        }
                    }
                );
            ServiceCollection
                .AddHttpClient<ITopicListService, TopicListService>(
                    (services, client) =>
                    {
                        client.BaseAddress = services
                            .GetRequiredService<AppSetting>()
                            .Api.TopicListUri;
                    }
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
                            socketsHttpHandler.UseProxy = appSetting.Api.IsSystemProxyEnabled;
                            socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (
                                sender,
                                cert,
                                chain,
                                sslPolicyErrors
                            ) =>
                            {
                                return appSetting.Api.IsCertificateVerificationEnabled
                                    || sslPolicyErrors == SslPolicyErrors.None;
                            };
                        }
                    }
                );
            ServiceCollection
                .AddHttpClient<ITopicService, TopicService>(
                    (services, client) =>
                    {
                        client.BaseAddress = services
                            .GetRequiredService<AppSetting>()
                            .Api.TopicDetailUri;
                    }
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
                            socketsHttpHandler.UseProxy = appSetting.Api.IsSystemProxyEnabled;
                            socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (
                                sender,
                                cert,
                                chain,
                                sslPolicyErrors
                            ) =>
                            {
                                return appSetting.Api.IsCertificateVerificationEnabled
                                    || sslPolicyErrors == SslPolicyErrors.None;
                            };
                        }
                    }
                );
            ServiceCollection
                .AddHttpClient<IUserService, UserService>(
                    (services, client) =>
                    {
                        client.BaseAddress = services
                            .GetRequiredService<AppSetting>()
                            .Api.UserDetailUri;
                    }
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
                            socketsHttpHandler.UseProxy = appSetting.Api.IsSystemProxyEnabled;
                            socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (
                                sender,
                                cert,
                                chain,
                                sslPolicyErrors
                            ) =>
                            {
                                return appSetting.Api.IsCertificateVerificationEnabled
                                    || sslPolicyErrors == SslPolicyErrors.None;
                            };
                        }
                    }
                );

            return ServiceCollection;
        }
    }
}
