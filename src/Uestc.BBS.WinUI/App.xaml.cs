using System;
using System.IO;
using System.Text.Json;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.WinUI.Services;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;
using WinUIEx;

namespace Uestc.BBS.WinUI
{
    public partial class App : Application
    {
        public static Window? CurrentWindow { get; set; }

        public App()
        {
            InitializeComponent();

            // 防多开
            if (!AppInstance.FindOrRegisterForKey(AppDomain.CurrentDomain.FriendlyName).IsCurrent)
            {
                Environment.Exit(0);
            }

            ServiceExtension
                .ConfigureCommonServices()
                // Windows & Pages
                .AddSingleton<AuthWindow>()
                .AddSingleton<MainWindow>()
                .AddSingleton<AuthPage>()
                .AddSingleton<MainPage>()
                .AddSingleton<HomePage>()
                .AddSingleton<SectionsPage>()
                .AddSingleton<ServicesPage>()
                .AddSingleton<MomentsPage>()
                .AddSingleton<PostPage>()
                .AddSingleton<MessagesPage>()
                .AddSingleton<SettingsPage>()
                // ViewModels
                .AddSingleton<AuthViewModel>()
                .AddSingleton<MainViewModel>()
                .AddSingleton<HomeViewModel>()
                .AddSingleton<SettingsViewModel>()
                // Models
                .AddSingleton<AppSettingModel>()
                // Appmanifest
                .AddSingleton(appmanifest =>
                    JsonSerializer.Deserialize<Appmanifest>(
                        File.ReadAllText("Assets/appmanifest.json"),
                        Appmanifest.SerializerOptions
                    ) ?? throw new ArgumentNullException(nameof(appmanifest))
                )
                // Notification
                .AddSingleton<INotificationService>(n =>
                {
                    var appmanifest = ServiceExtension.Services.GetRequiredService<Appmanifest>();
                    var appIconUri = new Uri(
                        Path.Combine(
                            "file://",
                            AppDomain.CurrentDomain.BaseDirectory,
                            "Assets",
                            "Icons",
                            "app.ico"
                        )
                    );
                    return new NotificationService(appmanifest.Name, appIconUri);
                })
                // 自启动
                .AddSingleton<IStartupService>(services =>
                {
                    var startupInfo = new StartupInfo
                    {
                        Name = AppDomain.CurrentDomain.FriendlyName,
                        Description = $"{AppDomain.CurrentDomain.FriendlyName} startup service",
                        ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                        ApplicationPath = Environment.ProcessPath!,
                    };
                    return new WindowsStartupService(startupInfo);
                });
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                var appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
                if (!appSetting.Auth.IsUserAuthed)
                {
                    // 未授权时强制显示登录页面，不受静默启动选项影响
                    // 托盘图标附加于主窗口，未登录时如果不显示登录窗口，程序将无法退出
                    CurrentWindow = ServiceExtension.Services.GetRequiredService<AuthWindow>();
                    CurrentWindow.Activate();
                    return;
                }

                CurrentWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
                if (appSetting.Apperance.StartupAndShutdown.SlientStart)
                {
                    CurrentWindow.Hide(
                        appSetting.Apperance.StartupAndShutdown.WindowCloseBehavior
                            is WindowCloseBehavior.HideWithEfficiencyMode
                    );
                    return;
                }
                CurrentWindow.Activate();
            }
            catch (Exception e)
            {
                ServiceExtension
                    .Services.GetRequiredService<ILogService>()
                    .Error("Application launched failed", e);
            }
        }
    }
}
