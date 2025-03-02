using System.Text.Json.Serialization;

namespace Uestc.BBS.Core.Services.Forum
{
    public interface ITopicService
    {
        /// <summary>
        /// 获取指定主题的详细信息
        /// </summary>
        /// <param name="topicId">主题 ID</param>
        /// <param name="authorId">回复作者 ID</param>
        /// <param name="reverseReplyOrder">回帖是否倒序排列</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TopicResp?> GetTopicAsync(
            uint topicId,
            uint? authorId = null,
            bool reverseReplyOrder = false,
            CancellationToken cancellationToken = default
        );
    }

    public class TopicResp : ApiRespBase
    {
        /// <summary>
        /// 版块
        /// </summary>
        [JsonPropertyName("boardId")]
        public Board Board { get; set; }

        /// <summary>
        /// 版块名称
        /// </summary>
        [JsonPropertyName("forumName")]
        public string BoardName { get; set; } = string.Empty;

        /// <summary>
        /// 主题
        /// </summary>
        public Topic Topic { get; set; } = new();

        /// <summary>
        /// 回帖列表
        /// </summary>
        [JsonPropertyName("list")]
        public Reply[] ReplyList { get; set; } = [];

        /// <summary>
        /// 主题源地址
        /// </summary>
        [JsonPropertyName("forumTopicUrl")]
        public string SourceUrl { get; set; } = string.Empty;
    }

    [JsonSerializable(typeof(TopicResp))]
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    )]
    public partial class TopicRespContext : JsonSerializerContext { }
}
