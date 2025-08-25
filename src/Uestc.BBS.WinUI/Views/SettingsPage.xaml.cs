using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.WinUI.Controls;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel { get; init; }

        private readonly ILogService _logService;

        private readonly INotificationService _notificationService;

        public SettingsPage(
            SettingsViewModel viewModel,
            ILogService logService,
            INotificationService notificationService
        )
        {
            InitializeComponent();

            ViewModel = viewModel;

            _logService = logService;
            _notificationService = notificationService;
        }

        private void OpenContributorHomepage(object sender, PointerRoutedEventArgs _)
        {
            if (sender is Avatar avatar && avatar.Tag is string homePage)
            {
                OperatingSystemHelper.OpenWebsite(homePage);
            }
        }
    }
}
