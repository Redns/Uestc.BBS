using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Auth;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.Desktop.ViewModels
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
            // 跳转至主页
            if (
                Application.Current?.ApplicationLifetime
                is not IClassicDesktopStyleApplicationLifetime desktop
            )
            {
                return;
            }

            var mainWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
            if (AppSettingModel.Services.StartupAndShutdown.SilentStart)
            {
                mainWindow.Hide();
                return;
            }

            // 显示主窗口
            mainWindow.Show();
            // 隐藏登录窗口
            desktop.MainWindow?.Hide();
            desktop.MainWindow = mainWindow;
        }
    }
}
