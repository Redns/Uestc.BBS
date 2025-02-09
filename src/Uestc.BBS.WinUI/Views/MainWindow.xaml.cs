using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.Helpers;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views.ContentDialogs;
using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class MainWindow : WindowEx
    {
        private readonly Appmanifest _appmanifest;

        private MainViewModel ViewModel { get; init; }

        public MainWindow(MainViewModel viewModel, Appmanifest appmanifest)
        {
            ViewModel = viewModel;

            _appmanifest = appmanifest;

            InitializeComponent();

            // 设置窗口位置
            this.CenterOnScreen();
            // 设置窗口图标
            this.SetIcon("Assets/Icons/app.ico");
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

            // 设置主题色
            this.SetThemeColor(viewModel.AppSettingModel.Apperance.ThemeColor);
            viewModel.AppSettingModel.Apperance.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(viewModel.AppSettingModel.Apperance.ThemeColor))
                {
                    this.SetThemeColor(viewModel.AppSettingModel.Apperance.ThemeColor);
                }
            };

            // 设置窗口关闭策略
            AppWindow.Closing += (window, args) =>
            {
                if (
                    viewModel.AppSettingModel.Apperance.StartupAndShutdown.WindowCloseBehavior
                    is not WindowCloseBehavior.Exit
                )
                {
                    this.Hide(
                        viewModel.AppSettingModel.Apperance.StartupAndShutdown.WindowCloseBehavior
                            is WindowCloseBehavior.HideWithEfficiencyMode
                    );
                    args.Cancel = true;
                }
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
                nameof(SectionsPage)
                    => ServiceExtension.Services.GetRequiredService<SectionsPage>(),
                nameof(ServicesPage)
                    => ServiceExtension.Services.GetRequiredService<ServicesPage>(),
                nameof(MomentsPage) => ServiceExtension.Services.GetRequiredService<MomentsPage>(),
                nameof(PostPage) => ServiceExtension.Services.GetRequiredService<PostPage>(),
                nameof(MessagesPage)
                    => ServiceExtension.Services.GetRequiredService<MessagesPage>(),
                nameof(SettingsPage)
                    => ServiceExtension.Services.GetRequiredService<SettingsPage>(),
                _
                    => throw new ArgumentException(
                        $"Navigate failed, unknown page {page}",
                        nameof(page)
                    ),
            };

        private void PersonPicture_PointerPressed(object sender, PointerRoutedEventArgs e)
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
        private async Task OpenAboutDialogAsync() =>
            await new ContentDialog
            {
                XamlRoot = Content.XamlRoot,
                Title = _appmanifest.Name,
                PrimaryButtonText = "确 定",
                Content = new AboutContentDialog()
                {
                    Version = _appmanifest.Version,
                    CopyRight = _appmanifest.CopyRight,
                    SourceRepositoryUrl = _appmanifest.SourceRepositoryUrl,
                },
            }.ShowAsync();
    }
}
