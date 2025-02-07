using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Mvvm.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="appSetting"></param>
    /// <param name="logService"></param>
    /// <param name="authService"></param>
    /// <param name="asyncNotififyFunction"></param>
    public abstract partial class AuthViewModelBase(
        AppSetting appSetting,
        ILogService logService,
        IAuthService authService,
        INotificationService notificationService
    ) : ObservableObject
    {
        public readonly ILogService _logService = logService;

        public readonly AppSetting _appSetting = appSetting;

        public readonly IAuthService _authService = authService;

        public readonly INotificationService _notificationService = notificationService;

        /// <summary>
        /// 用户名
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyPropertyChangedFor(nameof(UsernameMessage))]
        [NotifyPropertyChangedFor(nameof(SelectedCredential))]
        public partial string Username { get; set; } =
            appSetting.Auth.DefaultCredential?.Name ?? string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyPropertyChangedFor(nameof(PasswordMessage))]
        public partial string Password { get; set; } =
            appSetting.Auth.DefaultCredential?.Password ?? string.Empty;

        /// <summary>
        /// 自动登录
        /// </summary>
        [ObservableProperty]
        public partial bool AutoLogin { get; set; } = appSetting.Auth.AutoLogin;

        /// <summary>
        /// 记住密码
        /// </summary>
        [ObservableProperty]
        public partial bool RememberPassword { get; set; } = appSetting.Auth.RememberPassword;

        /// <summary>
        /// 选中的授权信息
        /// </summary>
        [ObservableProperty]
        public partial AuthCredential? SelectedCredential { get; set; }

        public bool CanLogin() =>
            !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        /// <summary>
        /// 提示信息
        /// </summary>
        public string UsernameMessage => Username == string.Empty ? "请输入用户名" : string.Empty;

        public string PasswordMessage => Password == string.Empty ? "请输入密码" : string.Empty;

        public void OnSelectCredentialChanged(AuthCredential credential)
        {
            Password = credential.Password;
            SelectedCredential = credential;
        }

        /// <summary>
        /// 打开官方论坛链接
        /// </summary>
        [RelayCommand]
        private void OpenOfficialWebsite()
        {
            OperatingSystemHelper.OpenWebsite(_appSetting.Apperance.OfficialWebsite);
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        public async Task LoginAsync()
        {
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
                NavigateToMainView();
            }
            catch (Exception e)
            {
                _logService.Error("Login failed", e);
                _notificationService.Show("登陆失败", e.Message);
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
                _notificationService.Show("登陆失败", "用户不存在或密码错误");
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

        /// <summary>
        /// 导航至首页
        /// </summary>
        public abstract void NavigateToMainView();
    }
}
