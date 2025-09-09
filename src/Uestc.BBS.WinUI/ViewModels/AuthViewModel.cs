using System;
using H.NotifyIcon;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Auth;
using Uestc.BBS.WinUI.Views;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class AuthViewModel(
        ILogService logService,
        [FromKeyedServices(ServiceExtensions.WEB_API)] IAuthService webAuthService,
        [FromKeyedServices(ServiceExtensions.MOBCENT_API)] IAuthService mobcentAuthService,
        INotificationService notificationService,
        Uri baseUri,
        AppSettingModel appSettingModel
    )
        : AuthViewModelBase(
            logService,
            webAuthService,
            mobcentAuthService,
            notificationService,
            baseUri,
            appSettingModel
        )
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
                _logService.Error("Navigate to main view failed", ex);
            }
            finally
            {
                authWindow?.Close();
            }
        }
    }
}
