using System.Collections.Generic;
using FastEnumUtility;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Models;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class ApperanceSettingsOverlay : Page
    {
        private ApperanceSettingsViewModel ViewModel { get; init; }

        private List<ThemeColor> ThemeColors { get; init; } = [.. FastEnum.GetValues<ThemeColor>()];

        public ApperanceSettingsOverlay(ApperanceSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
