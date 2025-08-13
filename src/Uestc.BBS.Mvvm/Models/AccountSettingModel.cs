using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Sdk.Services.Auth;

namespace Uestc.BBS.Mvvm.Models
{
    /// <summary>
    /// 授权设置
    /// </summary>
    public class AccountSettingModel : ObservableObject
    {
        private readonly AccountSetting _authSetting;

        public AccountSettingModel(AccountSetting authSetting)
        {
            _authSetting = authSetting;

            Credentials = [.. _authSetting.Credentials.Select(c => new AuthCredentialModel(c))];
            Credentials.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        authSetting.Credentials.AddRange(
                            args.NewItems!.Cast<AuthCredentialModel>().Select(c => c.AuthCredential)
                        );
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (
                            var item in args.OldItems!.Cast<AuthCredentialModel>()
                                .Select(c => c.AuthCredential)
                        )
                        {
                            authSetting.Credentials.Remove(item);
                        }
                        break;
                    default:
                        throw new ArgumentException(
                            "Unhandled collection change action.",
                            nameof(args)
                        );
                }
            };
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        public bool AutoLogin
        {
            get => _authSetting.AutoLogin;
            set =>
                SetProperty(_authSetting.AutoLogin, value, _authSetting, (s, e) => s.AutoLogin = e);
        }

        /// <summary>
        /// 记住密码（取消记住密码仍然会保存密钥信息）
        /// </summary>
        public bool RememberPassword
        {
            get => _authSetting.RememberPassword;
            set =>
                SetProperty(
                    _authSetting.RememberPassword,
                    value,
                    _authSetting,
                    (s, e) => s.RememberPassword = e
                );
        }

        /// <summary>
        /// 默认授权信息 Uid
        /// </summary>
        public uint DefaultCredentialUid
        {
            get => _authSetting.DefaultCredentialUid;
            set
            {
                SetProperty(
                    _authSetting.DefaultCredentialUid,
                    value,
                    _authSetting,
                    (s, e) => s.DefaultCredentialUid = e
                );
                OnPropertyChanged(nameof(IsUserAuthed));
                OnPropertyChanged(nameof(DefaultCredential));
            }
        }

        /// <summary>
        /// 默认授权信息
        /// </summary>
        public AuthCredentialModel? DefaultCredential =>
            Credentials.FirstOrDefault(c => c.Uid == DefaultCredentialUid);

        /// <summary>
        /// 用戶是否授权
        /// </summary>
        public bool IsUserAuthed => _authSetting.IsUserAuthed;

        /// <summary>
        /// 授权信息列表（保存本地所有授权信息）
        /// </summary>
        public ObservableCollection<AuthCredentialModel> Credentials { get; init; }
    }

    public class AuthCredentialModel(AuthCredential authCredential) : ObservableObject
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthCredential AuthCredential => authCredential;

        /// <summary>
        /// Uid
        /// </summary>
        public uint Uid
        {
            get => authCredential.Uid;
            set => SetProperty(authCredential.Uid, value, authCredential, (s, e) => s.Uid = e);
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Username
        {
            get => authCredential.Username;
            set =>
                SetProperty(
                    authCredential.Username,
                    value,
                    authCredential,
                    (s, e) => s.Username = e
                );
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => authCredential.Password;
            set =>
                SetProperty(
                    authCredential.Password,
                    value,
                    authCredential,
                    (s, e) => s.Password = e
                );
        }

        /// <summary>
        /// Token
        /// </summary>
        public string Token
        {
            get => authCredential.Token;
            set => SetProperty(authCredential.Token, value, authCredential, (s, e) => s.Token = e);
        }

        /// <summary>
        /// Secret
        /// </summary>
        public string Secret
        {
            get => authCredential.Secret;
            set =>
                SetProperty(authCredential.Secret, value, authCredential, (s, e) => s.Secret = e);
        }

        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie
        {
            get => authCredential.Cookie;
            set =>
                SetProperty(authCredential.Cookie, value, authCredential, (s, e) => s.Cookie = e);
        }

        /// <summary>
        /// Authorization
        /// </summary>
        public string Authorization
        {
            get => authCredential.Authorization;
            set =>
                SetProperty(
                    authCredential.Authorization,
                    value,
                    authCredential,
                    (s, e) => s.Authorization = e
                );
        }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar
        {
            get => authCredential.Avatar;
            set =>
                SetProperty(authCredential.Avatar, value, authCredential, (s, e) => s.Avatar = e);
        }

        /// <summary>
        /// 用户等级
        /// </summary>
        public uint Level
        {
            get => authCredential.Level;
            set => SetProperty(authCredential.Level, value, authCredential, (s, e) => s.Level = e);
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public string Group
        {
            get => authCredential.Group;
            set => SetProperty(authCredential.Group, value, authCredential, (s, e) => s.Group = e);
        }

        /// <summary>
        /// 用户签名
        /// </summary>
        public string Signature
        {
            get => authCredential.Signature;
            set =>
                SetProperty(
                    authCredential.Signature,
                    value,
                    authCredential,
                    (s, e) => s.Signature = e
                );
        }
    }
}
