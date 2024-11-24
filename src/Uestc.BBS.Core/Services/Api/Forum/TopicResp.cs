using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public class TopicResp : ApiRespBase
    {
        public int IsOnlyTopicType { get; set; }

        public int Page { get; set; }

        public int HasNext { get; set; }

        public int TotalNum { get; set; }

        public TopicOverview[] List { get; set; } = [];

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(ForumRespContext.Default, new DefaultJsonTypeInfoResolver())
        };
    }

    [JsonSerializable(typeof(TopicResp))]
    public partial class ForumRespContext : JsonSerializerContext
    {
    }
}
