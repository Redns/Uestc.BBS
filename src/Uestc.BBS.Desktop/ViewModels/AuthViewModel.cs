using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Labs.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Models;
using Uestc.BBS.Desktop.Views;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly AppSetting _appSetting;

        private readonly HttpClient _httpClient;

        private readonly ILogService _logService;

        private readonly IAuthService _authService;

        private readonly INativeNotification _notification;

        [ObservableProperty]
        private AppSettingModel _appSettingModel;

        /// <summary>
        /// 用户名
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyPropertyChangedFor(nameof(UsernameMessage))]
        private string? _username;

        /// <summary>
        /// 密码
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyPropertyChangedFor(nameof(PasswordMessage))]
        private string? _password;

        public string UsernameMessage => Username == string.Empty ? "请输入用户名" : string.Empty;

        public string PasswordMessage => Password == string.Empty ? "请输入密码" : string.Empty;

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

        [ObservableProperty]
        private bool _isLoging = false;

        /// <summary>
        /// 用户头像
        /// </summary>
        public string? Avatar => SelectedCredential?.Avatar;

        public bool CanLogin() =>
            string.IsNullOrEmpty(Username) is false && string.IsNullOrEmpty(Password) is false;

        /// <summary>
        /// 选中的本地授权信息
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Avatar))]
        private AuthCredential? _selectedCredential;

        /// <summary>
        /// 本地所有授权信息
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<AuthCredential> _users;

        public AuthViewModel(
            AppSetting appSetting,
            HttpClient httpClient,
            AppSettingModel appSettingModel,
            ILogService logService,
            IAuthService authService,
            INativeNotification nativeNotification
        )
        {
            _appSetting = appSetting;
            _httpClient = httpClient;
            _appSettingModel = appSettingModel;
            _logService = logService;
            _authService = authService;
            _notification = nativeNotification;

            AutoLogin = appSetting.Auth.AutoLogin;
            RememberPassword = appSetting.Auth.RememberPassword;
            SelectedCredential = appSetting.Auth.DefaultCredential;
            Users = [.. appSetting.Auth.Credentials.OrderBy(c => c.Name)];
        }

        [RelayCommand]
        private void SelectCredential(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count is not 0)
            {
                Password = e.AddedItems.Cast<AuthCredential>().First().Password;
                return;
            }
            Password = string.Empty;
        }

        /// <summary>
        /// 移除本地授权信息
        /// </summary>
        /// <param name="credential"></param>
        [RelayCommand]
        private void DeleteCredential(AuthCredential credential)
        {
            Users.Remove(credential);
            if (_appSetting.Auth.Credentials.Remove(credential))
            {
                _appSetting.Save();
            }
        }

        /// <summary>
        /// 打开官方论坛链接
        /// </summary>
        [RelayCommand]
        private void OpenOfficialWebsite()
        {
            Process.Start(
                new ProcessStartInfo()
                {
                    FileName = _appSetting.Apperance.OfficialWebsite,
                    UseShellExecute = true,
                }
            );
        }

        [RelayCommand]
        private async Task EnterToLoginAsync(KeyEventArgs args)
        {
            if (args.PhysicalKey is PhysicalKey.Enter && CanLogin())
            {
                await LoginAsync();
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync()
        {
            IsLoging = true;

            try
            {
                var credential = await GetAuthCredentialAsync();
                if (credential is null)
                {
                    return;
                }

                // 保存本地登录信息
                if (SelectedCredential?.Equals(credential) is not true)
                {
                    _appSetting.Auth.Credentials.Add(credential);
                }
                _appSetting.Auth.DefaultCredentialUid = credential.Uid;
                _appSetting.Auth.AutoLogin = AutoLogin;
                _appSetting.Auth.RememberPassword = RememberPassword;
                _appSetting.Save();

                // 跳转至主页
                if (
                    Application.Current?.ApplicationLifetime
                    is IClassicDesktopStyleApplicationLifetime desktop
                )
                {
                    var mainWindow = ServiceExtension.Services.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                    desktop.MainWindow?.Hide();
                    desktop.MainWindow = mainWindow;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("User login failed", ex);

                _notification.Title = "登陆失败";
                _notification.Message = ex.Message;
                _notification.Show();
            }
            finally
            {
                IsLoging = false;
            }
        }

        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <returns></returns>
        private async Task<AuthCredential?> GetAuthCredentialAsync()
        {
            // 选中授权信息有效，无需重新获取
            if (
                string.IsNullOrEmpty(SelectedCredential?.Token) is false
                && string.IsNullOrEmpty(SelectedCredential?.Secret) is false
                && SelectedCredential?.Password == Password
            )
            {
                return SelectedCredential;
            }

            // 根据用户名和密码登录
            var resp = await _authService.LoginAsync(Username!, Password!);
            if (resp?.Success is not true)
            {
                _notification.Title = "登陆失败";
                _notification.Message = "用户不存在或密码错误";
                _notification.Show();
                return null;
            }

            var credential =
                SelectedCredential
                ?? new AuthCredential
                {
                    Uid = resp.Uid,
                    Avatar = resp.Avatar,
                    Name = resp.Username,
                };
            credential.Password = RememberPassword ? Password! : string.Empty;
            credential.Token = resp.Token;
            credential.Secret = resp.Secret;

            return credential;
        }
    }
}
