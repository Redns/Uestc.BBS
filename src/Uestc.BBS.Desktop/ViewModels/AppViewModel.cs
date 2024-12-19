using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.Models;
using Uestc.BBS.Desktop.Views;

namespace Uestc.BBS.ViewModels
{
    public partial class AppViewModel : ObservableObject
    {
        [ObservableProperty]
        private AppSettingsModel _model;

        public AppViewModel(AppSettingsModel model)
        {
            _model = model;
        }

        /// <summary>
        /// 显示主窗口
        /// </summary>
        [RelayCommand]
        private void ShowMainWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow is null)
                {
                    var appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
                    desktop.MainWindow = appSetting.Auth.IsUserAuthed
                        ? ServiceExtension.Services.GetRequiredService<MainWindow>()
                        : ServiceExtension.Services.GetRequiredService<AuthWindow>();
                }

                desktop.MainWindow!.WindowState = WindowState.Normal;
                desktop.MainWindow.Show();
                desktop.MainWindow.Activate();
            }
        }

        /// <summary>
        /// 重启应用
        /// </summary>
        [RelayCommand]
        private void Restart()
        {
            Process.Start(
                new ProcessStartInfo(Environment.ProcessPath!)
                {
                    UseShellExecute = true,
                    Arguments = "restart"
                }
            );
            Environment.Exit(0);
        }

        /// <summary>
        /// 退出应用
        /// </summary>
        [RelayCommand]
        private static void Exit()
        {
            if (
                Application.Current?.ApplicationLifetime
                is IClassicDesktopStyleApplicationLifetime application
            )
            {
                application.Shutdown();
            }
        }
    }
}
