using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.Api.Auth;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    /// <summary>
    /// 登录视图模型基类
    /// </summary>
    public abstract partial class AuthViewModelBase : ObservableObject
    {
        protected readonly ILogService _logService;

        protected readonly IAuthService _authService;

        protected readonly INotificationService _notificationService;

        public AppSettingModel AppSettingModel { get; init; }

        /// <summary>
        /// 用户名
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyPropertyChangedFor(nameof(UsernameMessage))]
        [NotifyPropertyChangedFor(nameof(SelectedCredential))]
        public partial string? Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [NotifyPropertyChangedFor(nameof(PasswordMessage))]
        public partial string? Password { get; set; }

        /// <summary>
        /// 选中的授权信息
        /// </summary>
        public AuthCredentialModel? SelectedCredential =>
            AppSettingModel.Auth.Credentials.FirstOrDefault(c => c.Name == Username);

        /// <summary>
        /// 用户名或密码为空时不能登录
        /// </summary>
        public bool CanLogin => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        /// <summary>
        /// 提示信息
        /// </summary>
        public string UsernameMessage => Username == string.Empty ? "请输入用户名" : string.Empty;

        public string PasswordMessage => Password == string.Empty ? "请输入密码" : string.Empty;

        protected AuthViewModelBase(
            ILogService logService,
            IAuthService authService,
            INotificationService notificationService,
            AppSettingModel appSettingModel
        )
        {
            _logService = logService;
            _authService = authService;
            _notificationService = notificationService;

            AppSettingModel = appSettingModel;
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Username) && SelectedCredential is not null)
                {
                    Password = SelectedCredential.Password;
                }
            };
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanLogin))]
        public async Task LoginAsync()
        {
            try
            {
                // 获取授权信息
                var credential = await GetAuthCredentialAsync();
                if (credential is null)
                {
                    return;
                }

                // 保存本地登录信息
                if (SelectedCredential?.Equals(credential) is false)
                {
                    AppSettingModel.Auth.Credentials.Add(credential);
                }
                AppSettingModel.Auth.DefaultCredentialUid = credential.Uid;

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
        private async Task<AuthCredentialModel?> GetAuthCredentialAsync()
        {
            // 选中授权信息有效，无需重新获取
            if (
                !string.IsNullOrEmpty(SelectedCredential?.Token)
                && !string.IsNullOrEmpty(SelectedCredential?.Secret)
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

            // 若授权信息不存在则创建
            var credential =
                SelectedCredential
                ?? new AuthCredentialModel(
                    new AuthCredential
                    {
                        Uid = resp.Uid,
                        Avatar = resp.Avatar,
                        Name = resp.Username,
                    }
                );
            credential.Token = resp.Token;
            credential.Secret = resp.Secret;
            credential.Password = AppSettingModel.Auth.RememberPassword ? Password! : string.Empty;

            return credential;
        }

        /// <summary>
        /// 删除授权信息
        /// </summary>
        /// <param name="credential"></param>
        [RelayCommand]
        private void DeleteAuthCredential(AuthCredentialModel credential) =>
            AppSettingModel.Auth.Credentials.Remove(credential);

        /// <summary>
        /// 导航至首页
        /// </summary>
        public abstract void NavigateToMainView();

        /// <summary>
        /// 打开官方论坛链接
        /// </summary>
        [RelayCommand]
        private void OpenOfficialWebsite() =>
            OperatingSystemHelper.OpenWebsite(AppSettingModel.Apperance.OfficialWebsite);
    }
}
