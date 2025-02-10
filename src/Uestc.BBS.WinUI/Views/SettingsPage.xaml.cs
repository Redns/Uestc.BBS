using System;
using System.Collections.Generic;
using FastEnumUtility;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.WinUI.Helpers;
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
        private List<ThemeColor> ThemeColors { get; init; } = [.. FastEnum.GetValues<ThemeColor>()];

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

        private void OpenContributorHomepage(object sender, PointerRoutedEventArgs e)
        {
            if (sender is PersonPicture picture && picture.Tag is string homePage)
            {
                OperatingSystemHelper.OpenWebsite(homePage);
            }
        }

        private void ToggleStartupOnLaunch(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleSwitch toggleSwitch)
            {
                return;
            }

            if (
                toggleSwitch.IsOn
                == ViewModel.AppSettingModel.Apperance.StartupAndShutdown.StartupOnLaunch
            )
            {
                return;
            }

            try
            {
                _startupService.SetStartup(toggleSwitch.IsOn);
                ViewModel.AppSettingModel.Apperance.StartupAndShutdown.StartupOnLaunch =
                    toggleSwitch.IsOn;
            }
            catch (UnauthorizedAccessException)
            {
                toggleSwitch.IsOn = ViewModel
                    .AppSettingModel
                    .Apperance
                    .StartupAndShutdown
                    .StartupOnLaunch;
                _ = App.CurrentWindow?.RequireAdministratorPermissionAsync();
            }
            catch (Exception ex)
            {
                toggleSwitch.IsOn = ViewModel
                    .AppSettingModel
                    .Apperance
                    .StartupAndShutdown
                    .StartupOnLaunch;
                _notificationService.Show(
                    $"{(toggleSwitch.IsOn ? "开启" : "关闭")}自启动服务失败",
                    ex.Message
                );
                _logService.Error("Startup service set failed", ex);
            }
        }
    }
}
