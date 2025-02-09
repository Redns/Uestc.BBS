using CommunityToolkit.Mvvm.Input;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.WinUI.Views;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class AuthViewModel(
        ILogService logService,
        IAuthService authService,
        INotificationService notificationService,
        AppSettingModel appSettingModel
    ) : AuthViewModelBase(logService, authService, notificationService, appSettingModel)
    {
        public override void NavigateToMainView()
        {
            App.CurrentWindow?.Hide();
            if (!AppSettingModel.Apperance.StartupAndShutdown.SlientStart)
            {
                App.CurrentWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
                App.CurrentWindow.Activate();
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
