using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.Pages;
using Uestc.BBS.WinUI.Services.NavigateService;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            ServiceExtension.ConfigureCommonServices();
            // Windows & Pages & ViewModels
            ServiceExtension.ServiceCollection.AddTransient<MainWindow>();
            ServiceExtension.ServiceCollection.AddTransient<MainViewModel>();
            ServiceExtension.ServiceCollection.AddTransient<HomePage>();
            ServiceExtension.ServiceCollection.AddTransient<SectionsPage>();
            ServiceExtension.ServiceCollection.AddTransient<ServicesPage>();
            ServiceExtension.ServiceCollection.AddTransient<MomentsPage>();
            ServiceExtension.ServiceCollection.AddTransient<PostPage>();
            ServiceExtension.ServiceCollection.AddTransient<MessagesPage>();
            ServiceExtension.ServiceCollection.AddTransient<SettingsPage>();
            // Navigate
            ServiceExtension.ServiceCollection.AddSingleton<INavigateService, NavigateService>();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var mainWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
            mainWindow.Activate();
        }
    }
}
