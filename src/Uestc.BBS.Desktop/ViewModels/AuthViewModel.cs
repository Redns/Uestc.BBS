using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Uestc.BBS.Desktop.Views;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class AuthViewModel(MainWindow mainWindow) : ObservableObject
    {
        private readonly MainWindow _mainWindow = mainWindow;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

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
