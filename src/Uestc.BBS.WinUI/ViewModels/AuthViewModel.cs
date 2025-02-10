using H.NotifyIcon;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.WinUI.Views;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class AuthViewModel(
        MainWindow mainWindow,
        ILogService logService,
        IAuthService authService,
        INotificationService notificationService,
        AppSettingModel appSettingModel
    ) : AuthViewModelBase(logService, authService, notificationService, appSettingModel)
    {
        private readonly MainWindow _mainWindow = mainWindow;

        public override void NavigateToMainView()
        {
            App.CurrentWindow?.Close();
            App.CurrentWindow = _mainWindow;
            if (AppSettingModel.Apperance.StartupAndShutdown.SlientStart)
            {
                App.CurrentWindow.Hide();
                return;
            }
            App.CurrentWindow.Activate();
        }
    }
}
