using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel { get; init; }

        public HomePage(HomeViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
