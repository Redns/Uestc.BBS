using System.Security.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.Auth;

namespace Uestc.BBS.Mvvm.ViewModels
{
    /// <summary>
    /// 登录视图模型基类
    /// </summary>
    public abstract partial class AuthViewModelBase : ObservableObject
    {
        protected readonly ILogService _logService;

        protected readonly IAuthService _webAuthService;

        protected readonly IAuthService _mobcentAuthService;

        protected readonly INotificationService _notificationService;

        public Uri BaseUri { get; init; }

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
            AppSettingModel.Account.Credentials.FirstOrDefault(c => c.Username == Username);

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
            IAuthService webAuthService,
            IAuthService mobcentAuthService,
            INotificationService notificationService,
            Uri baseUri,
            AppSettingModel appSettingModel
        )
        {
            _logService = logService;
            _webAuthService = webAuthService;
            _mobcentAuthService = mobcentAuthService;
            _notificationService = notificationService;

            BaseUri = baseUri;
            AppSettingModel = appSettingModel;
            Username = appSettingModel.Account.DefaultCredential?.Username;
            Password = appSettingModel.Account.DefaultCredential?.Password;

            PropertyChanged += (_, args) =>
            {
                // 根据所选用户名自动填充密码
                if (args.PropertyName == nameof(Username) && SelectedCredential is not null)
                {
                    Password = SelectedCredential.Password;
                }
            };

            // 自动登录
            // FIXME 同步操作将导致 UI 卡顿，待修复
            if (appSettingModel.Account.AutoLogin && CanLogin)
            {
                LoginCommand.Execute(null);
            }
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
                var credential = await GetAuthCredentialAsync()
                    .TimeoutCancelAsync(TimeSpan.FromSeconds(5));

                // 保存本地登录信息
                if (SelectedCredential?.Equals(credential) is not true)
                {
                    AppSettingModel.Account.Credentials.Add(credential);
                }
                AppSettingModel.Account.DefaultCredentialUid = credential.Uid;

                // 跳转至主页
                NavigateToMainView();
            }
            catch (TimeoutException)
            {
                _notificationService.Show("登陆失败", "网络连接超时，请检查网络连接或稍后重试");
            }
            catch (AuthenticationException)
            {
                _notificationService.Show("登陆失败", "用户名或密码错误");
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
        private async Task<AuthCredentialModel> GetAuthCredentialAsync()
        {
            // 选中授权信息有效，无需重新获取
            if (
                SelectedCredential?.IsCookieAuthenticated is true
                && SelectedCredential.IsMobcentAuthenticated
            )
            {
                return SelectedCredential;
            }

            // 若授权信息不存在则创建
            var credential =
                SelectedCredential
                ?? new AuthCredentialModel(new AuthCredential { Username = Username! });
            credential.Password = Password!;

            // 获取 Cookie
            if (!credential.IsCookieAuthenticated)
            {
                await _webAuthService.LoginAsync(credential.AuthCredential);
            }

            // 获取 Mobcent Token & Secret
            if (!credential.IsMobcentAuthenticated)
            {
                await _mobcentAuthService.LoginAsync(credential.AuthCredential);
            }

            credential.Password = AppSettingModel.Account.RememberPassword
                ? Password!
                : string.Empty;

            return credential;
        }

        /// <summary>
        /// 删除授权信息
        /// </summary>
        /// <param name="credential"></param>
        [RelayCommand]
        private void DeleteAuthCredential(AuthCredentialModel credential) =>
            AppSettingModel.Account.Credentials.Remove(credential);

        /// <summary>
        /// 导航至首页
        /// </summary>
        public abstract void NavigateToMainView();
    }
}
