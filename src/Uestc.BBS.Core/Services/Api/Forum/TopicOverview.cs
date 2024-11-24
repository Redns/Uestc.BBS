using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public class TopicOverview
    {
        public int BoardId { get; set; }

        public string BoardName { get; set; } = string.Empty;

        public int TopicId { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string UserNickName { get; set; } = string.Empty;

        public string UserAvatar { get; set; } = string.Empty;

        public DateTime LastReplyDate { get; set; }

        public int Vote { get; set; }

        public int Hot { get; set; }

        public int Hits { get; set; }

        public int Replies { get; set; }

        public int Essence { get; set; }

        public int Top { get; set; }

        public int Status { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string PicPath { get; set; } = string.Empty;

        public string Ratio { get; set; } = string.Empty;

        public int Gender { get; set; }

        public string UserTitle { get; set; } = string.Empty;

        public int RecommendAdd { get; set; }

        public int Special { get; set; }

        public int IsHasRecommendAdd { get; set; }

        public string[] ImageList { get; set; } = [];

        public string SourceWebUrl { get; set; } = string.Empty;

        public string[] Verify { get; set; } = [];


        public static readonly JsonSerializerOptions SerializeOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(TopicOverviewContext.Default, new DefaultJsonTypeInfoResolver())
        };
    }

    [JsonSerializable(typeof(TopicOverview))]
    public partial class TopicOverviewContext : JsonSerializerContext
    {
    }
}
