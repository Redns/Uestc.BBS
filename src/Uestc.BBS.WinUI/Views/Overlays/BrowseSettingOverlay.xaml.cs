using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class BrowseSettingOverlay : Page
    {
        private BrowseSettingsViewModel ViewModel { get; init; }

        public BrowseSettingOverlay(BrowseSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
