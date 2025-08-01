using System.Text.Json.Serialization;

namespace Uestc.BBS.Core.Services.User
{
    public partial class User { }

    /// <summary>
    /// WHAT'S THIS?
    /// https://bbs.uestc.edu.cn/forum.php?mod=viewthread&tid=2351215
    /// "verify": [
    ///  {
    ///    "icon": "https://bbs.uestc.edu.cn/data/attachment/common/c4/common_1_verify_icon.png",
    ///    "vid": 1,
    ///    "verifyName": "vip"
    ///  }
    ///]
    /// </summary>
    public class UserVerify
    {
        public int Vid { get; set; }

        [JsonPropertyName("verifyName")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string IconUrl { get; set; } = string.Empty;
    }

    public static class UserExtension
    {
        public static uint GetUserTitleLevel(this string userTitle)
        {
            if (uint.TryParse(userTitle.Split('.').Last()[0..^1], out var level))
            {
                return level;
            }

            throw new ArgumentException("Unable to parse level from user title", nameof(userTitle));
        }

        public static string GetUserTitleAlias(this string userTitle) =>
            userTitle.Split('（').First();
    }
}
