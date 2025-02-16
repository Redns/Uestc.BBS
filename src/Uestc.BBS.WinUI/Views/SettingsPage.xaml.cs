using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging;
using FastEnumUtility;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel { get; init; }

        private readonly ILogService _logService;

        private readonly IStartupService _startupService;

        private readonly INotificationService _notificationService;

        #region 枚举值列表
        private List<LogLevel> LogLevels { get; init; } = [.. FastEnum.GetValues<LogLevel>()];

        private List<WindowCloseBehavior> WindowCloseBehaviors { get; init; } =
            [.. FastEnum.GetValues<WindowCloseBehavior>()];
        #endregion

        public SettingsPage(
            SettingsViewModel viewModel,
            ILogService logService,
            IStartupService startupService,
            INotificationService notificationService
        )
        {
            InitializeComponent();

            ViewModel = viewModel;

            _logService = logService;
            _startupService = startupService;
            _notificationService = notificationService;
        }

        private void OpenContributorHomepage(object sender, PointerRoutedEventArgs _)
        {
            if (sender is PersonPicture picture && picture.Tag is string homePage)
            {
                OperatingSystemHelper.OpenWebsite(homePage);
            }
        }

        private void SettingsCard_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            StrongReferenceMessenger.Default.Send(
                new NavigateChangedMessage(MenuItemKey.ApperanceSettings)
            );
        }
    }
}
