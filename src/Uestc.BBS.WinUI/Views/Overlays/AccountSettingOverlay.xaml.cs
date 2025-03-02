using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class AccountSettingOverlay : Page
    {
        private AccountSettingsViewModel ViewModel { get; init; }

        public AccountSettingOverlay(AccountSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
