using System;
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

        private void OpenPersonalCenterFlyout(object sender, PointerRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}
