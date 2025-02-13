using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel { get; init; } =
            ServiceExtension.Services.GetRequiredService<MainViewModel>();

        public MainPage()
        {
            InitializeComponent();
        }

        private void OpenPersonalCenterFlyout(object sender, PointerRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void ClearTopMenuSelection(
            NavigationView sender,
            NavigationViewItemInvokedEventArgs args
        ) => TopMenuBar.SelectedIndex = -1;
    }
}
