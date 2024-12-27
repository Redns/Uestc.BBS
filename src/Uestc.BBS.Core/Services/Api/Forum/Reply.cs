using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

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

        /// <summary>
        /// 序列化参数
        /// </summary>
        public static readonly JsonSerializerOptions SerializeOptions =
            new()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = JsonTypeInfoResolver.Combine(
                    ReplyContext.Default,
                    new DefaultJsonTypeInfoResolver()
                )
            };
    }

    [JsonSerializable(typeof(Reply))]
    public partial class ReplyContext : JsonSerializerContext { }
}
