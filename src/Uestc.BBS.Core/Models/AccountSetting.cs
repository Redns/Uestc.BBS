using System.Text.Json.Serialization;
using Uestc.BBS.Sdk.Services.Auth;

namespace Uestc.BBS.Core.Models
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
}
