using CommunityToolkit.Mvvm.Input;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Core.ViewModels;
using Uestc.BBS.WinUI.Views;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class AuthViewModel(
        AppSetting appSetting,
        ILogService logService,
        IAuthService authService,
        INotificationService notificationService
    ) : AuthViewModelBase(appSetting, logService, authService, notificationService)
    {
        public override void NavigateToMainView()
        {
            ServiceExtension.Services.GetRequiredService<AuthWindow>().Hide();
            if (!_appSetting.Apperance.SlientStart)
            {
                ServiceExtension.Services.GetRequiredService<MainWindow>().Activate();
            }
        }

        [RelayCommand]
        public void SelectCredential(AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is AuthCredential credential)
            {
                OnSelectCredentialChanged(credential);
            }
        }
    }
}
