using System;
using System.IO;
using System.Text.Json;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.WinUI.Services;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;
using Uestc.BBS.WinUI.Views.Overlays;
using Windows.UI.ViewManagement;
using WinUIEx;

namespace Uestc.BBS.WinUI
{
    public partial class App : Application
    {
        private static readonly UISettings _uiSettings = new();

        public static ThemeColor SystemTheme =>
            _uiSettings.GetColorValue(UIColorType.Background).R > 128
                ? ThemeColor.Light
                : ThemeColor.Dark;

        public static event Action<UISettings, ThemeColor> SystemThemeChanged = delegate { };

        public static Window? CurrentWindow { get; set; }

        public App()
        {
            InitializeComponent();

            // 防多开
            if (!AppInstance.FindOrRegisterForKey(AppDomain.CurrentDomain.FriendlyName).IsCurrent)
            {
                Environment.Exit(0);
            }

            // 依赖注入
            ServiceExtension
                .ConfigureCommonServices()
                // Windows
                .AddTransient<AuthWindow>()
                .AddTransient<MainWindow>()
                // Pages
                .AddTransient<AuthPage>()
                .AddSingleton<MainPage>()
                .AddSingleton<HomePage>()
                .AddSingleton<SectionsPage>()
                .AddSingleton<ServicesPage>()
                .AddSingleton<MomentsPage>()
                .AddSingleton<PostPage>()
                .AddSingleton<MessagesPage>()
                .AddTransient<SettingsPage>()
                // Overlays
                .AddTransient<MyFavoritesOverlay>()
                .AddTransient<MyPostsOverlay>()
                .AddTransient<MyRepliesOverlay>()
                .AddTransient<MyMarksOverlay>()
                .AddTransient<TopicFilterOverlay>()
                // ViewModels
                .AddTransient<AuthViewModel>()
                .AddTransient<MainViewModel>()
                .AddSingleton<HomeViewModel>()
                .AddSingleton<SectionsViewModel>()
                .AddSingleton<ServicesViewModel>()
                .AddSingleton<MomentsViewModel>()
                .AddSingleton<PostViewModel>()
                .AddSingleton<MessagesViewModel>()
                .AddTransient<SettingsViewModel>()
                .AddTransient<MyFavoritesViewModel>()
                .AddTransient<MyPostsViewModel>()
                .AddTransient<MyRepliesViewModel>()
                .AddTransient<MyMarksViewModel>()
                .AddTransient<TopicFilterViewModel>()
                // Models
                .AddSingleton<AppSettingModel>()
                // Appmanifest
                .AddSingleton(services =>
                    JsonSerializer.Deserialize(
                        File.ReadAllText("Assets/appmanifest.json"),
                        AppmanifestContext.Default.Appmanifest
                    ) ?? throw new ArgumentNullException()
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
                // Navigate
                .AddSingleton<INavigateService<Page>>(services => new NavigateService(services))
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

            // 监听系统主题变更
            _uiSettings.ColorValuesChanged += (sender, args) =>
            {
                SystemThemeChanged(_uiSettings, SystemTheme);
            };

            // 应用退出时保存设置
            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
                ServiceExtension.Services.GetRequiredService<AppSetting>().Save();

            // 捕获并记录未处理异常
            Current.UnhandledException += (sender, args) =>
            {
                ServiceExtension
                    .Services.GetRequiredService<ILogService>()
                    .Error(args.Message, args.Exception);
                ServiceExtension.Services.GetRequiredService<AppSetting>().Save();
            };
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
                if (appSetting.Apperance.StartupAndShutdown.SilentStart)
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
