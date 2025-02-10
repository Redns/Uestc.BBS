using System.Text.Json.Serialization;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public class Reply
    {
        /// <summary>
        /// 用户 ID
        /// </summary>
        public string Uid { get; set; } = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 回复 ID
        /// </summary>
        public string ReplyId { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }

    [JsonSerializable(typeof(Reply))]
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    )]
    public partial class ReplyContext : JsonSerializerContext { }
}
