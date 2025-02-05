using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.Helpers;
using Uestc.BBS.WinUI.ViewModels;
using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class MainWindow : WindowEx
    {
        private readonly AppSetting _appSetting;

        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel, AppSetting appSetting)
        {
            _viewModel = viewModel;
            _appSetting = appSetting;

            InitializeComponent();

            // 设置窗口位置
            this.CenterOnScreen();
            // 拓展内容至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置标题栏高度
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

            // 设置侧边栏默认导航选项
            navigateView.SelectedItem = navigateView.MenuItems[0];
            if (navigateView.SelectedItem is NavigationViewItem menu && menu.Tag is string page)
            {
                navigateFrame.Content = NavigateToPage(page);
            }

            // 设置窗口关闭策略
            AppWindow.Closing += (window, args) =>
            {
                if (_appSetting.Apperance.WindowCloseBehavior is WindowCloseBehavior.Exit)
                {
                    return;
                }

                args.Cancel = true;
                this.Hide(
                    _appSetting.Apperance.WindowCloseBehavior
                        is WindowCloseBehavior.HideWithEfficiencyMode
                );
            };
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

        [RelayCommand]
        private void Restart() => WindowsHelper.Restart();

        [RelayCommand]
        private void Exit() => WindowsHelper.Exit();

        [RelayCommand]
        private void ShowMainView()
        {
            if (AppWindow.IsVisible)
            {
                BringToFront();
                return;
            }
            this.Show();
        }

        [RelayCommand]
        private Task OpenAboutDialogAsync()
        {
            //var dialog = new ContentDialog
            //{
            //    XamlRoot = Content.XamlRoot,
            //    Title = "Save your work?",
            //    PrimaryButtonText = "确定",
            //    DefaultButton = ContentDialogButton.Primary,
            //    Content = "aaa",
            //};

            //var result = await dialog.ShowAsync();
            return ShowMessageDialogAsync("Hello World!", "Dialog title");
        }
    }
}
