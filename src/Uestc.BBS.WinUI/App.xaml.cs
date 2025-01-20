﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Uestc.BBS.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
