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
            ServiceCollection.AddHttpClient();
            ServiceCollection
                .AddDailySentencesService(services =>
                    services.GetRequiredService<AppSetting>().Services.Network.BaseUri
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
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
                        }
                    }
                );
            ServiceCollection
                .AddAuthService(services =>
                    services.GetRequiredService<AppSetting>().Services.Network.BaseUri
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
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
                        }
                    }
                );
            ServiceCollection
                .AddMobcentThreadListService(services =>
                    services.GetRequiredService<AppSetting>().Services.Network.BaseUri
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
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
                        }
                    }
                );
            ServiceCollection
                .AddMobcentThreadContentService(services =>
                    services.GetRequiredService<AppSetting>().Services.Network.BaseUri
                )
                .ConfigurePrimaryHttpMessageHandler(
                    (handler, services) =>
                    {
                        var appSetting = services.GetRequiredService<AppSetting>();
                        if (handler is SocketsHttpHandler socketsHttpHandler)
                        {
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
                        }
                    }
                );

            return ServiceCollection;
        }
    }
}
