using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public partial class AuthPage : Page
    {
        private AuthViewModel ViewModel { get; init; } =
            ServiceExtension.Services.GetRequiredService<AuthViewModel>();

        public AuthPage()
        {
            InitializeComponent();
        }

        private void DeleteAuthCredential(object sender, RoutedEventArgs e)
        {
            if (
                sender is not AppBarButton button
                || button.Tag is not AuthCredentialModel credential
            )
            {
                return;
            }

            ViewModel.DeleteAuthCredentialCommand.Execute(credential);
            if (UsernameAutoSuggestBox.ItemsSource is List<AuthCredentialModel> credentials)
            {
                UsernameAutoSuggestBox.ItemsSource = credentials
                    .Where(c => !string.IsNullOrEmpty(c.Username))
                    .Where(c => c.Username != credential.Username)
                    .ToList();
            }
        }

        /// <summary>
        /// 根据用户输入过滤本地授权信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void FilterAuthCredentials(
            AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args
        )
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            {
                return;
            }

            var newCredentials = ViewModel
                .AppSettingModel.Account.Credentials.Where(c => !string.IsNullOrEmpty(c.Username))
                .Where(u => u.Username.Contains(sender.Text, StringComparison.OrdinalIgnoreCase));
            if (
                sender.ItemsSource is not List<AuthCredentialModel> oldCredentials
                || oldCredentials.Count != newCredentials.Count()
                || newCredentials.Except(oldCredentials).Any()
            )
            {
                sender.ItemsSource = newCredentials.ToList();
            }
        }

        /// <summary>
        /// 选择本地授权信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SelectAuthCredential(
            AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args
        )
        {
            if (args.SelectedItem is AuthCredentialModel credential)
            {
                sender.Text = credential.Username;
            }
        }

        private async void EnterToLogin(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key is Windows.System.VirtualKey.Enter && ViewModel.CanLogin)
            {
                await ViewModel.LoginAsync();
            }
        }
    }
}
