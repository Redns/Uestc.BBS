using System.Text.Json.Serialization;
using Uestc.BBS.Core.JsonConverters;

namespace Uestc.BBS.Core.Services.Forum.TopicList
{
    public interface ITopicListService
    {
        Task<TopicListResp?> GetTopicsAsync(
            string? route = null,
            uint page = 1,
            uint pageSize = 10,
            uint moduleId = 2,
            Board boardId = 0,
            TopicSortType sortby = TopicSortType.New,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop,
            bool getPreviewSources = false,
            bool getPartialReply = false,
            CancellationToken cancellationToken = default
        );
    }

    public class TopicListResp : ApiRespBase
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

    [JsonSerializable(typeof(TopicListResp))]
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    )]
    public partial class TopicRespContext : JsonSerializerContext
    {
    }
}
