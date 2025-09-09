using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
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

    public class AuthCredentialModel : ObservableObject
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthCredential AuthCredential { get; init; }

        public AuthCredentialModel(AuthCredential authCredential)
        {
            AuthCredential = authCredential;
            BlacklistUsers = [.. authCredential.BlacklistUsers];
            BlacklistUsers.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                        authCredential.BlacklistUsers.Clear();
                        break;
                    case NotifyCollectionChangedAction.Add:
                        authCredential.BlacklistUsers.AddRange(
                            args.NewItems!.Cast<BlacklistUser>()
                        );
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in args.OldItems!.Cast<BlacklistUser>())
                        {
                            authCredential.BlacklistUsers.Remove(item);
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
        /// Uid
        /// </summary>
        public uint Uid
        {
            get => AuthCredential.Uid;
            set => SetProperty(AuthCredential.Uid, value, AuthCredential, (s, e) => s.Uid = e);
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Username
        {
            get => AuthCredential.Username;
            set =>
                SetProperty(
                    AuthCredential.Username,
                    value,
                    AuthCredential,
                    (s, e) => s.Username = e
                );
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => AuthCredential.Password;
            set =>
                SetProperty(
                    AuthCredential.Password,
                    value,
                    AuthCredential,
                    (s, e) => s.Password = e
                );
        }

        /// <summary>
        /// Token
        /// </summary>
        public string Token
        {
            get => AuthCredential.Token;
            set
            {
                SetProperty(AuthCredential.Token, value, AuthCredential, (s, e) => s.Token = e);
                OnPropertyChanged(nameof(IsMobcentAuthenticated));
            }
        }

        /// <summary>
        /// Secret
        /// </summary>
        public string Secret
        {
            get => AuthCredential.Secret;
            set
            {
                SetProperty(AuthCredential.Secret, value, AuthCredential, (s, e) => s.Secret = e);
                OnPropertyChanged(nameof(IsMobcentAuthenticated));
            }
        }

        /// <summary>
        /// Cookie
        /// </summary>
        public CookieCollection Cookies
        {
            get => AuthCredential.Cookies;
            set
            {
                SetProperty(AuthCredential.Cookies, value, AuthCredential, (s, e) => s.Cookies = e);
                OnPropertyChanged(nameof(IsCookieAuthenticated));
            }
        }

        public CookieContainer CookieContainer
        {
            get => AuthCredential.CookieContainer;
        }

        /// <summary>
        /// Authorization
        /// </summary>
        public string Authorization
        {
            get => AuthCredential.Authorization;
            set
            {
                SetProperty(
                    AuthCredential.Authorization,
                    value,
                    AuthCredential,
                    (s, e) => s.Authorization = e
                );
                OnPropertyChanged(nameof(IsCookieAuthenticated));
            }
        }

        /// <summary>
        /// 是否已通过 Cookie 验证
        /// </summary>
        public bool IsCookieAuthenticated => AuthCredential.IsCookieAuthenticated;

        /// <summary>
        /// 是否已通过 Mobcent 验证
        /// </summary>
        public bool IsMobcentAuthenticated => AuthCredential.IsMobcentAuthenticated;

        /// <summary>
        /// 用户等级
        /// </summary>
        public uint Level
        {
            get => AuthCredential.Level;
            set => SetProperty(AuthCredential.Level, value, AuthCredential, (s, e) => s.Level = e);
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public string Group
        {
            get => AuthCredential.Group;
            set => SetProperty(AuthCredential.Group, value, AuthCredential, (s, e) => s.Group = e);
        }

        /// <summary>
        /// 用户签名
        /// </summary>
        public string Signature
        {
            get => AuthCredential.Signature;
            set =>
                SetProperty(
                    AuthCredential.Signature,
                    value,
                    AuthCredential,
                    (s, e) => s.Signature = e
                );
        }

        /// <summary>
        /// 黑名单用户列表
        /// </summary>
        public ObservableCollection<BlacklistUser> BlacklistUsers { get; init; }
    }
}
