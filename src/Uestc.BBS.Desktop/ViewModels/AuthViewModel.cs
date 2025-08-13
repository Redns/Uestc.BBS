using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.Sdk.Services.Auth;

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

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task EnterToLoginAsync(KeyEventArgs args)
        {
            if (args.PhysicalKey is PhysicalKey.Enter)
            {
                await LoginAsync();
            }
        }
    }
}
