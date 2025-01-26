using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Uestc.BBS.Core;
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
            ServiceExtension.ServiceCollection.AddTransient<MainWindow>();
            ServiceExtension.ServiceCollection.AddTransient<MainViewModel>();
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
