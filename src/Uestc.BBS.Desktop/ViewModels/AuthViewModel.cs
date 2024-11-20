using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Uestc.BBS.Desktop.Views;
using System.Collections.Generic;
using Uestc.BBS.Core;
using System.Linq;
using Avalonia.Controls;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class AuthViewModel(MainWindow mainWindow, AppSetting appSetting) : ObservableObject
    {
        private readonly MainWindow _mainWindow = mainWindow;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private AuthCredential[] _users = [.. appSetting.Auth.Credentials.OrderBy(u => u.Name)];

        [RelayCommand]
        private void Login()
        {
            // TODO 用户授权验证

            // TODO 保存授权信息至本地

            // 跳转至主页面
            var applicationLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (applicationLifetime is not null)
            {
                _mainWindow.Show();
                applicationLifetime.MainWindow?.Hide();
                applicationLifetime.MainWindow = _mainWindow; 
            }
        }
    }
}
