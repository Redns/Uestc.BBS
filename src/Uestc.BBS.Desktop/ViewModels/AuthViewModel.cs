using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Core;
using System.Linq;
using System.Diagnostics;
using Avalonia.Media.Imaging;
using Uestc.BBS.Desktop.Helpers;
using System.Threading.Tasks;
using System.Net.Http;
using Uestc.BBS.Core.Services.Api.Auth;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly MainWindow _mainWindow;

        private readonly AppSetting _appSetting;

        private readonly HttpClient _httpClient;

        private readonly IAuthService _authService;

        /// <summary>
        /// 用户名
        /// </summary>
        [ObservableProperty]
        private string _username =  string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [ObservableProperty]
        private string _password = string.Empty;

        /// <summary>
        /// 记住密码
        /// </summary>
        [ObservableProperty]
        private bool _rememberPassword;

        /// <summary>
        /// 自动登录
        /// </summary>
        [ObservableProperty]
        private bool _autoLogin;

        /// <summary>
        /// 用户头像
        /// </summary>
        public Task<Bitmap?> Avatar => ImageHelper.LoadFromWeb(_httpClient, _selectedCredential?.Avatar);

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
                }
                Password = _selectedCredential?.Password ?? string.Empty;
                OnPropertyChanged(nameof(Avatar));
            }
        }

        /// <summary>
        /// 本地所有授权信息
        /// </summary>
        [ObservableProperty]
        private AuthCredential[] _users;

        public AuthViewModel(MainWindow mainWindow, AppSetting appSetting, HttpClient httpClient, IAuthService authService)
        {
            _mainWindow = mainWindow;
            _appSetting = appSetting;
            _authService = authService;
            _httpClient = httpClient;
            AutoLogin = appSetting.Auth.AutoLogin;
            RememberPassword = appSetting.Auth.RememberPassword;
            SelectedCredential = appSetting.Auth.DefaultCredential;
            Users = [.. appSetting.Auth.Credentials.OrderBy(c => c.Name)];
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task Login()
        {
            if (!string.IsNullOrEmpty(_selectedCredential?.Token) && !string.IsNullOrEmpty(_selectedCredential?.Secret))
            {
                NavigateToMainPage();
                return;
            }

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                // NOTI 用户名和密码不能为空
                return;
            }

            // 根据用户名和密码登录
            var resp = await _authService.LoginAsync(Username, Password);
            if (resp?.Success is null or false)
            {
                // NOTI 用户名不存在或密码错误
                return;
            }

            // 更新本地登录信息
            var credential = SelectedCredential ?? new AuthCredential 
            {
                Uid = resp.Uid,
                Name = resp.Username,
                Avatar = resp.Avatar,
                Password = string.Empty
            };
            credential.Token = resp.Token;
            credential.Secret = resp.Secret;
            credential.Password = RememberPassword ? Password : credential.Password;

            // 保存本地登录信息
            if (_selectedCredential is null)
            {
                _appSetting.Auth.Credentials.Add(credential);
            }
            _appSetting.Auth.DefaultCredentialUid = resp.Uid;
            _appSetting.Auth.AutoLogin = AutoLogin;
            _appSetting.Auth.RememberPassword = RememberPassword;
            _appSetting.Save();

            // 跳转至主页
            NavigateToMainPage();
        }

        private void NavigateToMainPage()
        {
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
