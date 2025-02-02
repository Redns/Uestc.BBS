using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Core.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class AuthViewModel(
        AppSetting appSetting,
        ILogService logService,
        IAuthService authService
    ) : AuthViewModelBase(appSetting, logService, authService)
    {
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
