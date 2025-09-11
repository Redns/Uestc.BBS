using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.WinUI.Helpers;
using Uestc.BBS.WinUI.Services;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;
using Uestc.BBS.WinUI.Views.ContentDialogs;
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

        public static WindowEx? CurrentWindow { get; set; }

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
                .AddTransient<MainPage>()
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
                .AddTransient<AppearanceSettingOverlay>()
                .AddTransient<BrowseSettingOverlay>()
                .AddTransient<AccountSettingOverlay>()
                .AddTransient<NotificationSettingOverlay>()
                .AddTransient<StorageSettingOverlay>()
                .AddTransient<ServicesSettingOverlay>()
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
                .AddTransient<AppearanceSettingsViewModel>()
                .AddTransient<BrowseSettingsViewModel>()
                .AddTransient<AccountSettingsViewModel>()
                .AddTransient<ServicesSettingsViewModel>()
                .AddTransient<StorageSettingViewModel>()
                // Models
                .AddSingleton<AppSettingModel>()
                // Appmanifest
                .AddSingleton(services =>
                    JsonSerializer.Deserialize(
                        File.ReadAllText(Path.Combine("Assets", "appmanifest.json")),
                        AppmanifestContext.Default.Appmanifest
                    ) ?? throw new ArgumentNullException(nameof(Appmanifest))
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
                // Capture
                .AddTransient<ICaptureService<UIElement>, CaptureService>()
                // FileCache
                .AddSingleton<IFileCache>(services =>
                {
                    var cacheRoot = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        AppDomain.CurrentDomain.FriendlyName,
                        "Cache"
                    );
                    Directory.CreateDirectory(cacheRoot);

                    return new LocalFileCache(
                        cacheRoot,
                        services.GetRequiredService<IHttpClientFactory>()
                    );
                });

            // 监听系统主题变更
            _uiSettings.ColorValuesChanged += (_, _) =>
            {
                SystemThemeChanged(_uiSettings, SystemTheme);
            };

            // 应用退出时保存设置
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
                ServiceExtension.Services.GetRequiredService<AppSetting>().Save();

            #region 异常处理

            // 不导致崩溃但未处理的异常
            UnhandledException += async (_, args) =>
            {
                args.Handled = true;

                ServiceExtension
                    .Services.GetRequiredService<ILogService>()
                    .Error(args.Message, args.Exception);
                ServiceExtension.Services.GetRequiredService<AppSetting>().Save();

                if (CurrentWindow is null)
                {
                    Exit();
                }

                var dialog = new AppAbortDialog { Exception = args.Exception };
                await new ContentDialog
                {
                    XamlRoot = CurrentWindow!.Content.XamlRoot,
                    Title = "发生未知错误",
                    Content = dialog,
                    CloseButtonText = "确 定",
                    DefaultButton = ContentDialogButton.Close,
                }.ShowAsync();

                if (dialog.Feedback && !string.IsNullOrEmpty(dialog.FeekbackContent))
                {
                    // TODO 发送反馈
                }

                if (dialog.RestartApp)
                {
                    WindowsHelper.Restart();
                }
            };

            // 崩溃性错误，多是和框架相关的 COMException
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                if (args.ExceptionObject is not Exception e)
                {
                    return;
                }

                ServiceExtension.Services.GetRequiredService<ILogService>().Error(e.Message, e);
                ServiceExtension.Services.GetRequiredService<AppSetting>().Save();

                if (CurrentWindow is null)
                {
                    Exit();
                }
            };

            TaskScheduler.UnobservedTaskException += (_, args) =>
            {
                args.SetObserved();
                ServiceExtension
                    .Services.GetRequiredService<ILogService>()
                    .Error("Unobserved task exception", args.Exception);
                ServiceExtension.Services.GetRequiredService<AppSetting>().Save();
            };

            #endregion
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                var appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
                if (!appSetting.Account.IsUserAuthed)
                {
                    // 未授权时强制显示登录页面，不受静默启动选项影响
                    // 托盘图标附加于主窗口，未登录时如果不显示登录窗口，程序将无法退出
                    CurrentWindow = ServiceExtension.Services.GetRequiredService<AuthWindow>();
                    CurrentWindow.Activate();
                    return;
                }

                CurrentWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
                if (appSetting.Services.StartupAndShutdown.SilentStart)
                {
                    CurrentWindow.Hide(
                        appSetting.Services.StartupAndShutdown.WindowCloseBehavior
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
