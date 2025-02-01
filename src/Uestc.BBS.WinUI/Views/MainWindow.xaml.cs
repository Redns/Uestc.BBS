using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Uestc.BBS.Core.Services.NavigateService;
using Uestc.BBS.WinUI.ViewModels;
using WinRT.Interop;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class MainWindow : Window
    {
        private readonly AppWindow m_AppWindow;

        private readonly MainViewModel _viewModel;

        private readonly INavigateService _navigateService;

        public MainWindow(MainViewModel viewModel, INavigateService navigateService)
        {
            _viewModel = viewModel;
            _navigateService = navigateService;
            
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            navigateView.SelectedItem = navigateView.MenuItems[0];
            if (navigateView.SelectedItem is NavigationViewItem menu)
            {
                navigateFrame.Content = navigateService.Navigate(
                    menu.Tag as string
                        ?? throw new Exception(
                            $"MainWindow init failed, default navigate page is null"
                        )
                );
            }

            // 设置窗口大小
            m_AppWindow = GetCurrentWindow();
            m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            m_AppWindow.Resize(new Windows.Graphics.SizeInt32(1500, 900));
        }

        private AppWindow GetCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        /// <summary>
        /// 导航至页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentException"></exception>
        private void NavigateToPage(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.Tag is not string page)
            {
                return;
            }
            navigateFrame.Content = _navigateService.Navigate(page);
        }

        private void PersonPicture_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}
