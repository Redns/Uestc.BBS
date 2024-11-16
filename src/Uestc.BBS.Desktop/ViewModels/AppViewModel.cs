using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using System.Diagnostics;
using System;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Desktop;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Core;

namespace Uestc.BBS.ViewModels
{
    public partial class AppViewModel : ObservableObject
    {
        public AppViewModel()
        {
        }

        /// <summary>
        /// 显示主窗口
        /// </summary>
        [RelayCommand]
        private void ShowMainWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow ??= ServiceExtension.GetRequiredService<MainWindow>();
                desktop.MainWindow.WindowState = WindowState.Normal;
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
            Process.Start(new ProcessStartInfo(Environment.ProcessPath!)
            {
                UseShellExecute = true,
                Arguments = "restart"
            });
            Environment.Exit(0);
        }

        /// <summary>
        /// 退出应用
        /// </summary>
        [RelayCommand]
        private static void Exit()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                application.Shutdown();
            }
        }
    }
}