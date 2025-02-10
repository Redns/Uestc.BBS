using System.Collections.Generic;
using FastEnumUtility;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel { get; init; }

        private List<ThemeColor> ThemeColors { get; init; } = [.. FastEnum.GetValues<ThemeColor>()];

        private List<LogLevel> LogLevels { get; init; } = [.. FastEnum.GetValues<LogLevel>()];

        private List<WindowCloseBehavior> WindowCloseBehaviors { get; init; } =
            [.. FastEnum.GetValues<WindowCloseBehavior>()];

        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        private void OpenContributorHomepage(object sender, PointerRoutedEventArgs e)
        {
            if (sender is PersonPicture picture && picture.Tag is string homePage)
            {
                OperatingSystemHelper.OpenWebsite(homePage);
            }
        }
    }
}
