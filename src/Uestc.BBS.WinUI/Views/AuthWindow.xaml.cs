using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.ViewModels;
using Windows.Graphics;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class AuthWindow : Window
    {
        private readonly AppSetting _appSetting;

        private readonly AuthViewModel _viewModel;

        public AuthWindow(AppSetting appSetting, AuthViewModel viewModel)
        {
            InitializeComponent();
            InitCustomizeWindow();

            _viewModel = viewModel;
            _appSetting = appSetting;
        }

        private void InitCustomizeWindow()
        {
            // 内容拓展至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置窗口大小
            AppWindow.Resize(new SizeInt32(480, 400));
            // 隐藏标题栏
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
            // 设置可拖动区域
            SetTitleBar(AppTitleBar);
        }

        private void Username_AutoSuggestBox_TextChanged(
            AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args
        )
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            {
                return;
            }

            var newCredentials = _appSetting
                .Auth.Credentials.Where(u =>
                    u.Name.Contains(sender.Text, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();
            if (
                sender.ItemsSource is not List<AuthCredential> oldCredentials
                || oldCredentials.Count != newCredentials.Count
                || newCredentials.Except(oldCredentials).Any()
            )
            {
                sender.ItemsSource = newCredentials;
            }
        }

        private void CloseWindow(object sender, PointerRoutedEventArgs e) => Close();
    }
}
