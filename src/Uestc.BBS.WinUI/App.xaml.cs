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
using Uestc.BBS.WinUI.Services.Notifications;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;
using WinUIEx;

namespace Uestc.BBS.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
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
                });
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                var appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
                if (!appSetting.Auth.IsUserAuthed)
                {
                    // 未授权时强制显示登录页面，不受静默启动选项影响
                    // 托盘图标附加于主窗口，未登录时如果不显示登录窗口，程序将无法退出
                    ServiceExtension.Services.GetRequiredService<AuthWindow>().Activate();
                    return;
                }

                var mainWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
                if (appSetting.Apperance.SlientStart)
                {
                    mainWindow.Hide(
                        appSetting.Apperance.WindowCloseBehavior
                            is WindowCloseBehavior.HideWithEfficiencyMode
                    );
                    return;
                }
                mainWindow.Activate();
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
