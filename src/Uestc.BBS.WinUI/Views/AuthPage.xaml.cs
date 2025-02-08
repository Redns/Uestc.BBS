using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public partial class AuthPage : Page
    {
        private readonly AppSetting _appSetting;

        private readonly AuthViewModel _viewModel;

        public AuthPage()
        {
            InitializeComponent();

            _viewModel = ServiceExtension.Services.GetRequiredService<AuthViewModel>();
            _appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
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
    }
}
