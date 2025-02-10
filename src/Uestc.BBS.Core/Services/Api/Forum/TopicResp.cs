using System.Text.Json.Serialization;
using Uestc.BBS.Core.Helpers.JsonConverters;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public class TopicResp : ApiRespBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsOnlyTopicType { get; set; }

        /// <summary>
        /// 当前分页
        /// </summary>
        public uint Page { get; set; }

        /// <summary>
        /// 是否有下一分页
        /// </summary>
        public uint HasNext { get; set; }

        /// <summary>
        /// 总贴数
        /// </summary>
        public uint TotalNum { get; set; }

        /// <summary>
        /// 帖子摘要
        /// </summary>
        public TopicOverview[] List { get; set; } = [];
    }

    [JsonSerializable(typeof(TopicResp))]
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    )]
    public partial class TopicRespContext : JsonSerializerContext
    {
    }
}
