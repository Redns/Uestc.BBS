using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            _viewModel = viewModel;

            InitializeComponent();

            // 拓展内容至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置标题栏高度
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            // 设置窗口大小
            AppWindow.Resize(new Windows.Graphics.SizeInt32(1500, 900));

            // 设置侧边栏默认导航选项
            navigateView.SelectedItem = navigateView.MenuItems[0];
            if (navigateView.SelectedItem is NavigationViewItem menu && menu.Tag is string page)
            {
                navigateFrame.Content = NavigateToPage(page);
            }
        }

        /// <summary>
        /// 导航至页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentException"></exception>
        private void NavigateToPage(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.Tag is string page)
            {
                navigateFrame.Content = NavigateToPage(page);
            }
        }

        /// <summary>
        /// 导航至页面
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private Page NavigateToPage(string page) =>
            page switch
            {
                nameof(HomePage) => ServiceExtension.Services.GetRequiredService<HomePage>(),
                nameof(SectionsPage) =>
                    ServiceExtension.Services.GetRequiredService<SectionsPage>(),
                nameof(ServicesPage) =>
                    ServiceExtension.Services.GetRequiredService<ServicesPage>(),
                nameof(MomentsPage) => ServiceExtension.Services.GetRequiredService<MomentsPage>(),
                nameof(PostPage) => ServiceExtension.Services.GetRequiredService<PostPage>(),
                nameof(MessagesPage) =>
                    ServiceExtension.Services.GetRequiredService<MessagesPage>(),
                nameof(SettingsPage) =>
                    ServiceExtension.Services.GetRequiredService<SettingsPage>(),
                _ => throw new ArgumentException(
                    $"Navigate failed, unknown page {page}",
                    nameof(page)
                ),
            };

        private void PersonPicture_PointerPressed(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e
        )
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}
