using System;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
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
            var authWindow = App.CurrentWindow;

            try
            {
                App.CurrentWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
                if (AppSettingModel.Services.StartupAndShutdown.SilentStart)
                {
                    App.CurrentWindow.Hide();
                    return;
                }
                App.CurrentWindow.Activate();
            }
            catch (Exception ex)
            {
                _logService.Error("NavigateToMainView", ex);
            }
            finally
            {
                authWindow?.Close();
            }
        }
    }
}
