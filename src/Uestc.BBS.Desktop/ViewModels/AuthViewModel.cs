using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Core;
using System.Linq;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Uestc.BBS.Desktop.Helpers;
using System;
using System.Threading.Tasks;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly MainWindow _mainWindow;

        private readonly AppSetting _appSetting;

        /// <summary>
        /// 用户名
        /// </summary>
        [ObservableProperty]
        private string _username = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private Task<Bitmap?> _avatar;

        /// <summary>
        /// 选中的本地授权信息
        /// </summary>
        private AuthCredential? _selectedCredential;
        public AuthCredential? SelectedCredential
        {
            get { return _selectedCredential; }
            set
            {
                SetProperty(ref _selectedCredential, value);
                if (value is not null)
                {
                    Username = _selectedCredential?.Name ?? string.Empty;
                    Avatar = ImageHelper.LoadFromWeb(new Uri(SelectedCredential?.Avatar ?? string.Empty));
                }
                Password = _selectedCredential?.Password ?? string.Empty;
            }
        }

        /// <summary>
        /// 本地所有授权信息
        /// </summary>
        [ObservableProperty]
        private AuthCredential[] _users;

        public AuthViewModel(MainWindow mainWindow, AppSetting appSetting)
        {
            _mainWindow = mainWindow;
            _appSetting = appSetting;
            _users = [.. appSetting.Auth.Credentials.OrderBy(c => c.Name)];
            SelectedCredential = appSetting.Auth.Credentials.FirstOrDefault(c => c.Uid == appSetting.Auth.DefaultCredentialUid);
        }

        [RelayCommand]
        private void OpenOfficialWebsite()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = _appSetting.Apperance.OfficialUrl,
                UseShellExecute = true
            });
        }

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
