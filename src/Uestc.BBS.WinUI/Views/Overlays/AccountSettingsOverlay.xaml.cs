using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class AccountSettingsOverlay : Page
    {
        private AccountSettingsViewModel ViewModel { get; init; }

        public AccountSettingsOverlay(AccountSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
