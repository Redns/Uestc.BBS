using System.Text.Json.Serialization;

namespace Uestc.BBS.Core
{
    /// <summary>
    /// 授权设置
    /// </summary>
    public class AccountSetting
    {
        /// <summary>
        /// 自动登录
        /// </summary>
        public bool AutoLogin { get; set; } = false;

        /// <summary>
        /// 记住密码（取消记住密码仍然会保存密钥信息）
        /// </summary>
        public bool RememberPassword { get; set; } = false;

        /// <summary>
        /// 默认授权信息 Uid
        /// </summary>
        public uint DefaultCredentialUid { get; set; }

        /// <summary>
        /// 默认授权信息
        /// </summary>
        [JsonIgnore]
        public AuthCredential? DefaultCredential =>
            Credentials.FirstOrDefault(c => c.Uid == DefaultCredentialUid);

        /// <summary>
        /// 用戶是否授权
        /// </summary>
        [JsonIgnore]
        public bool IsUserAuthed =>
            !string.IsNullOrEmpty(DefaultCredential?.Token)
            && !string.IsNullOrEmpty(DefaultCredential.Secret);

        /// <summary>
        /// 授权信息列表（保存本地所有授权信息）
        /// </summary>
        public List<AuthCredential> Credentials { get; init; } = [];
    }

    /// <summary>
    /// 授权信息
    /// </summary>
    public class AuthCredential
    {
        /// <summary>
        /// 用户唯一识别码
        /// </summary>
        public uint Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 令牌
        /// </summary>

        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 密钥
        /// </summary>

        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// 用户等级
        /// </summary>
        public uint Level { get; set; } = 1;

        /// <summary>
        /// 用户组
        /// </summary>
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; } = string.Empty;

        /// <summary>
        /// 此处序列化用户 AutoCompleteBox 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
