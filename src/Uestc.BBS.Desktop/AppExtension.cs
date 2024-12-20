using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Threading;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Models;
using Uestc.BBS.Desktop.Services;
using Uestc.BBS.Desktop.Services.StartupService;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Desktop
{
    public static class AppExtension
    {
        /// <summary>
        /// 初始化桌面端服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AppBuilder ConfigureServices(this AppBuilder builder)
        {
            ServiceExtension.ConfigureCommonServices();

            // 自启动
            ServiceExtension.ServiceCollection.AddTransient<IStartupService>(startup =>
            {
                var startupInfo = new StartupInfo
                {
                    Name = AppDomain.CurrentDomain.FriendlyName,
                    Description = $"{AppDomain.CurrentDomain.FriendlyName} startup service",
                    ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    ApplicationPath = Environment.ProcessPath!
                };

                if (OperatingSystem.IsWindows())
                {
                    return new WindowsStartupService(startupInfo);
                }
                else if (OperatingSystem.IsLinux())
                {
                    return new LinuxStartupService(startupInfo);
                }
                else
                {
                    return new MacCatalystStartupService();
                }
            });
            // View & ViewModel
            ServiceExtension.ServiceCollection.AddSingleton<AppViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<AuthWindow>();
            ServiceExtension.ServiceCollection.AddSingleton<AuthViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<MainWindow>();
            ServiceExtension.ServiceCollection.AddSingleton<MainWindowViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<HomeView>();
            ServiceExtension.ServiceCollection.AddSingleton<SectionsView>();
            ServiceExtension.ServiceCollection.AddSingleton<ServicesView>();
            ServiceExtension.ServiceCollection.AddSingleton<MomentsView>();
            ServiceExtension.ServiceCollection.AddSingleton<PostView>();
            ServiceExtension.ServiceCollection.AddSingleton<MessagesView>();
            ServiceExtension.ServiceCollection.AddSingleton<SettingsView>();
            ServiceExtension.ServiceCollection.AddSingleton<AppSettingsModel>();
            ServiceExtension.ServiceCollection.AddSingleton<SettingsViewModel>();
            // HttpClient
            ServiceExtension.ServiceCollection.AddHttpClient<AuthViewModel>()
                .UseSocketsHttpHandler((handler, _) => handler.PooledConnectionLifetime = TimeSpan.FromMinutes(30))
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            ServiceExtension.ServiceCollection.AddHttpClient<MainWindowViewModel>()
                .UseSocketsHttpHandler((handler, _) => handler.PooledConnectionLifetime = TimeSpan.FromMinutes(30))
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            // 日志
            ServiceExtension.ServiceCollection.AddSingleton<ILogService>(logger =>
            {
                var nlogger = new NLogService(LogManager.GetLogger("*"));
                var appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
                nlogger.Setup(appSetting.Log);
                return nlogger;
            });
            // App upgrade
            ServiceExtension.ServiceCollection.AddSingleton<IAppUpgradeService>(appUpgrade => new CloudflareAppUpgradeService("https://distributor.krins.cloud",
                "679edd7cffaf4a9ef3be4c445317a461",
                "45fcd2ff239321d48ad4cae7ea9b5c4457f9d12f2483eb4029836931f5f83526",
                "distributor",
                "https://11f33fc072df859ebaf8faa0b0e1766b.r2.cloudflarestorage.com"));

            return builder;
        }
    }
}
