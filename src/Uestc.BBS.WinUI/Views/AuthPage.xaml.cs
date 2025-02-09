using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public partial class AuthPage : Page
    {
        private AuthViewModel ViewModel { get; init; }

        public AuthPage()
        {
            InitializeComponent();

            ViewModel = ServiceExtension.Services.GetRequiredService<AuthViewModel>();
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

            var newCredentials = ViewModel
                .AppSettingModel.Auth.Credentials.Where(u =>
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
    }
}
