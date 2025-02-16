using System.Collections.Generic;
using FastEnumUtility;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class ServicesSettingsOverlay : Page
    {
        private ServicesSettingsViewModel ViewModel { get; init; }

        private List<LogLevel> LogLevels { get; init; } = [.. FastEnum.GetValues<LogLevel>()];

        private List<WindowCloseBehavior> WindowCloseBehaviors { get; init; } =
            [.. FastEnum.GetValues<WindowCloseBehavior>()];

        public ServicesSettingsOverlay(ServicesSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
