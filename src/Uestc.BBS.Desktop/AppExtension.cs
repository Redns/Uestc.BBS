using System;
using System.Text.Json;
using System.Threading;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Helpers;
using Uestc.BBS.Desktop.Models;
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
            ServiceExtension.ServiceCollection.AddSingleton<AppSettingModel>();
            ServiceExtension.ServiceCollection.AddSingleton<SettingsViewModel>();
            // HttpClient
            ServiceExtension
                .ServiceCollection.AddHttpClient<AuthViewModel>()
                .UseSocketsHttpHandler(
                    (handler, _) => handler.PooledConnectionLifetime = TimeSpan.FromMinutes(30)
                )
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            ServiceExtension
                .ServiceCollection.AddHttpClient<MainWindowViewModel>()
                .UseSocketsHttpHandler(
                    (handler, _) => handler.PooledConnectionLifetime = TimeSpan.FromMinutes(30)
                )
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            ServiceExtension
                .ServiceCollection.AddHttpClient<SettingsViewModel>()
                .UseSocketsHttpHandler(
                    (handler, _) => handler.PooledConnectionLifetime = TimeSpan.FromMinutes(30)
                )
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            // Appmanifest
            ServiceExtension.ServiceCollection.AddSingleton(appmanifest =>
                JsonSerializer.Deserialize<Appmanifest>(
                    ResourceHelper.Load("/Assets/appmanifest.json"),
                    Appmanifest.SerializerOptions
                ) ?? throw new ArgumentNullException(nameof(appmanifest))
            );
            // 日志
            ServiceExtension.ServiceCollection.AddSingleton<ILogService>(logger =>
            {
                var nlogger = new NLogService(LogManager.GetLogger("*"));
                var appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
                nlogger.Setup(appSetting.Log);
                return nlogger;
            });
            // 自启动
            ServiceExtension.ServiceCollection.AddTransient<IStartupService>(startup =>
            {
                var startupInfo = new StartupInfo
                {
                    Name = AppDomain.CurrentDomain.FriendlyName,
                    Description = $"{AppDomain.CurrentDomain.FriendlyName} startup service",
                    ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    ApplicationPath = Environment.ProcessPath!,
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

            return builder;
        }
    }
}
