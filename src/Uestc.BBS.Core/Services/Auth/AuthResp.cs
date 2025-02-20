using System.Text.Json.Serialization;
using Uestc.BBS.Core.Services.User;

namespace Uestc.BBS.Core.Services.Api.Auth
{
    public class AuthResp : ApiRespBase
    {
        #region Authorization
        public int GroupId { get; set; } = 0;
        public int IsValidation { get; set; } = 0;

        public string Token { get; set; } = string.Empty;

        public string Secret { get; set; } = string.Empty;
        #endregion

        [JsonIgnore]
        public bool Success => Rs is 1;

        public int Score { get; set; } = 0;

        public uint Uid { get; set; } = 0;

        public string Username { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public int Gender { get; set; } = 0;
        public string Mobile { get; set; } = string.Empty;

        #region UserTitle
        public string UserTitle { get; set; } = string.Empty;

        [JsonIgnore]
        public uint UserTitleLevel => UserTitle.GetUserTitleLevel();

        [JsonIgnore]
        public string UserTitleAlias => UserTitle.GetUserTitleAlias();
        #endregion

        public Credit[] CreditShowList { get; set; } = [];
    }

    public class Credit
    {
        public string Type { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int Data { get; set; } = 0;
    }

    [JsonSerializable(typeof(AuthResp))]
    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
    public partial class AuthRespContext : JsonSerializerContext { }
}
