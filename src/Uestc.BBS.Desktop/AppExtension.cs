using System;
using System.Text.Json;
using Avalonia;
using Avalonia.Labs.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Helpers;
using Uestc.BBS.Desktop.Models;
using Uestc.BBS.Desktop.Services.StartupService;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Mvvm;
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
            ServiceExtension
                .ConfigureCommonServices()
                // Notification
                .AddTransient(services =>
                    NativeNotificationManager.Current?.CreateNotification(null)
                    ?? throw new Exception("Failed to create notification")
                )
                // View & ViewModel
                .AddSingleton<AppViewModel>()
                .AddSingleton<AuthWindow>()
                .AddSingleton<AuthViewModel>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<HomeView>()
                .AddSingleton<HomeViewModel>()
                .AddSingleton<SectionsView>()
                .AddSingleton<ServicesView>()
                .AddSingleton<MomentsView>()
                .AddSingleton<PostView>()
                .AddSingleton<MessagesView>()
                .AddSingleton<SettingsView>()
                .AddSingleton<AppSettingModel>()
                .AddSingleton<SettingsViewModel>()
                // Appmanifest
                .AddTransient<Appmanifest>(services =>
                    JsonSerializer.Deserialize<Appmanifest>(
                        ResourceHelper.Load("/Assets/appmanifest.json"),
                        AppmanifestContext.Default.Options
                    ) ?? throw new ArgumentNullException(nameof(Appmanifest))
                )
                // 自启动
                .AddTransient<IStartupService>(services =>
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
            // HttpClient
            ServiceExtension.ServiceCollection.AddHttpClient<AuthViewModel>();
            ServiceExtension.ServiceCollection.AddHttpClient<MainWindowViewModel>();
            ServiceExtension.ServiceCollection.AddHttpClient<SettingsViewModel>();

            return builder;
        }
    }
}
