using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class BrowseSettingsOverlay : Page
    {
        private BrowseSettingsViewModel ViewModel { get; init; }

        public BrowseSettingsOverlay(BrowseSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
