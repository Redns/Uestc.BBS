using System.Linq;
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

            ViewModel.PropertyChanging += (_, e) =>
            {
                if (
                    e.PropertyName == nameof(ViewModel.CurrentMenuItem)
                    && ViewModel.AppSettingModel.Appearance.MenuItems.Any(m =>
                        m.Key == ViewModel.CurrentMenuKey
                    )
                )
                {
                    NavigateBottom2TopStoryboard.Begin();
                    return;
                }

                if (
                    e.PropertyName == nameof(ViewModel.CurrentMenuKey)
                    && !ViewModel.AppSettingModel.Appearance.MenuItems.Any(m =>
                        m.Key == ViewModel.CurrentMenuKey
                    )
                )
                {
                    NavigateLeft2RightStoryboard.Begin();
                    return;
                }
            };

            ViewModel.PropertyChanged += (_, e) =>
            {
                if (
                    e.PropertyName == nameof(ViewModel.CurrentMenuKey)
                    && ViewModel.CurrentMenuItem.Key != ViewModel.CurrentMenuKey
                )
                {
                    NavigateRight2LeftStoryboard.Begin();
                    return;
                }
            };
        }

        /// <summary>
        /// 展开个人中心弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPersonalCenterFlyout(object sender, PointerRoutedEventArgs _) =>
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);

        /// <summary>
        /// 清除顶部菜单的选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ClearTopMenuSelection(
            NavigationView _,
            NavigationViewItemInvokedEventArgs e
        ) => TopMenuBar.SelectedIndex = -1;
    }
}
