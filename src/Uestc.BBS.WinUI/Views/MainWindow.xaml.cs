using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using H.NotifyIcon;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.WinUI.Helpers;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views.ContentDialogs;
using WinRT.Interop;
using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class MainWindow : WindowEx
    {
        private readonly Appmanifest _appmanifest;

        private AppSettingModel AppSettingModel { get; init; }

        public MainWindow(
            AppSettingModel appSettingModel,
            HomeViewModel homeViewModel,
            Appmanifest appmanifest
        )
        {
            InitializeComponent();

            AppSettingModel = appSettingModel;
            _appmanifest = appmanifest;

            // 设置窗口位置
            this.CenterOnScreen();
            // 设置窗口图标
            this.SetIcon("Assets/Icons/app.ico");
            // 拓展内容至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置标题栏高度
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

            // 设置主题色
            this.SetThemeColor(AppSettingModel.Appearance.ThemeColor);
            AppSettingModel.Appearance.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(AppSettingModel.Appearance.ThemeColor))
                {
                    this.SetThemeColor(AppSettingModel.Appearance.ThemeColor);
                }
            };
            App.SystemThemeChanged += (_, args) =>
            {
                if (AppSettingModel.Appearance.ThemeColor is ThemeColor.System)
                {
                    DispatcherQueue.EnqueueAsync(() => AppWindow.TitleBar.SetThemeColor(args));
                }
            };

            // 设置窗口关闭策略
            AppWindow.Closing += (_, args) =>
            {
                // 隐藏窗口/隐藏窗口+效率模式
                if (
                    AppSettingModel.Services.StartupAndShutdown.WindowCloseBehavior
                    is not WindowCloseBehavior.Exit
                )
                {
                    args.Cancel = true;
                    this.Hide(
                        AppSettingModel.Services.StartupAndShutdown.WindowCloseBehavior
                            is WindowCloseBehavior.HideWithEfficiencyMode
                    );
                }
            };

            homeViewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName != nameof(homeViewModel.CurrentThread))
                {
                    // 就业区防截屏
                    WindowsHelper.SetWindowDisplayAffinity(
                        WindowNative.GetWindowHandle(this),
                        homeViewModel.CurrentThread?.Board is Board.EmploymentAndEntrepreneurship
                            ? 0x11U
                            : 0x0U
                    );
                }
            };
        }

        [RelayCommand]
        private void ToggleSilentStart() =>
            AppSettingModel.Services.StartupAndShutdown.SilentStart = AppSettingModel
                .Services
                .StartupAndShutdown
                .SilentStart;

        [RelayCommand]
        private void ToggleDailysentenceEnabled() =>
            AppSettingModel.Appearance.SearchBar.IsDailySentenceEnabled = !AppSettingModel
                .Appearance
                .SearchBar
                .IsDailySentenceEnabled;

        [RelayCommand]
        private void Restart()
        {
            AppSettingModel.Save();
            WindowsHelper.Restart();
        }

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
                    FeedbackUrl = _appmanifest.FeedbackUrl,
                },
            }.ShowAsync();
    }
}
