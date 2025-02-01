using System;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.NavigateService;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Core.ViewModels;
using Uestc.BBS.WinUI.Services;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;

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

            ServiceExtension.ConfigureCommonServices();
            // Windows & Pages & ViewModels
            ServiceExtension.ServiceCollection.AddTransient<AuthWindow>();
            ServiceExtension.ServiceCollection.AddTransient<AuthViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<MainWindow>();
            ServiceExtension.ServiceCollection.AddSingleton<MainViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<HomePage>();
            ServiceExtension.ServiceCollection.AddSingleton<SectionsPage>();
            ServiceExtension.ServiceCollection.AddSingleton<ServicesPage>();
            ServiceExtension.ServiceCollection.AddSingleton<MomentsPage>();
            ServiceExtension.ServiceCollection.AddSingleton<PostPage>();
            ServiceExtension.ServiceCollection.AddSingleton<MessagesPage>();
            ServiceExtension.ServiceCollection.AddSingleton<SettingsPage>();
            // Navigate
            ServiceExtension.ServiceCollection.AddSingleton<INavigateService, NavigateService>();
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
                    // 托盘图标附加于主窗口，未登录时如果不显示主窗口，程序将无法退出
                    ServiceExtension.Services.GetRequiredService<AuthWindow>().Activate();
                    return;
                }

                if (appSetting.Apperance.SlientStart)
                {
                    ServiceExtension.Services.GetRequiredService<MainWindow>().Hide();
                    return;
                }
                ServiceExtension.Services.GetRequiredService<MainWindow>().Activate();
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
