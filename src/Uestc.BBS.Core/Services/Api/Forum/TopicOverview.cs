using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Uestc.BBS.Core.Helpers.JsonConverters;
using Uestc.BBS.Core.Services.Api.User;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    /// <summary>
    /// 帖子摘要
    /// </summary>
    public partial class TopicOverview
    {
        /// <summary>
        /// 板块 ID
        /// </summary>
        [JsonPropertyName("board_id")]
        public Board BoardId { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        [JsonIgnore]
        public string BoardName => BoardId.GetName();

        /// <summary>
        /// 帖子 ID
        /// </summary>
        [JsonPropertyName("topic_id")]
        public uint TopicId { get; set; }

        /// <summary>
        /// 帖子类型
        /// </summary>
        [JsonConverter(typeof(StringToTopicTypeConverter))]
        public TopicType Type { get; set; } = TopicType.Normal;

        /// <summary>
        /// 帖子标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 发帖用户 ID
        /// </summary>
        [JsonPropertyName("user_id")]
        public uint UserId { get; set; }

        /// <summary>
        /// 发帖用户名
        /// </summary>
        [JsonPropertyName("user_nick_name")]
        public string UserNickName { get; set; } = string.Empty;

        /// <summary>
        /// 发帖用户头像
        /// </summary>
        public string UserAvatar { get; set; } = string.Empty;

        /// <summary>
        /// 是否匿名发帖
        /// </summary>
        [JsonIgnore]
        public bool IsAnonymous => UserId is 0;

        /// <summary>
        /// 最新回复日期（Unix 毫秒级时间戳）
        /// </summary>
        [JsonPropertyName("last_reply_date")]
        [JsonConverter(typeof(UnixTimestampStringToDateTimeConverter))]
        public DateTime LastReplyDate { get; set; }

        /// <summary>
        /// 是否包含投票
        /// </summary>
        [JsonPropertyName("vote")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool HasVote { get; set; }

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public int Hot { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public uint Hits { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public uint Replies { get; set; }

        /// <summary>
        /// 是否为精华贴
        /// </summary>
        [JsonPropertyName("essence")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsEssence { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [JsonPropertyName("top")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsTop { get; set; }

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// 预览图链接
        /// </summary>
        [JsonPropertyName("pic_path")]
        public string PicPath { get; set; } = string.Empty;

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public string Ratio { get; set; } = string.Empty;

        /// <summary>
        /// TODO 枚举性别类型
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 用户头衔
        /// </summary>
        public string UserTitle { get; set; } = string.Empty;

        /// <summary>
        /// 用户等级
        /// </summary>
        public uint UserLevel => UserTitle.GetUserTitleLevel();

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public int RecommendAdd { get; set; }

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public int Special { get; set; }

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public int IsHasRecommendAdd { get; set; }

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public string[] ImageList { get; set; } = [];

        /// <summary>
        /// 源链接
        /// </summary>
        public string SourceWebUrl { get; set; } = string.Empty;

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public string[] Verify { get; set; } = [];

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public string[] ZanList { get; set; } = [];

        /// <summary>
        /// 回复列表
        /// </summary>
        [JsonPropertyName("Reply")]
        public Reply[] ReplyList { get; set; } = [];

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
                    TopicOverviewContext.Default,
                    new DefaultJsonTypeInfoResolver()
                )
            };
    }

    /// <summary>
    /// 帖子类型
    /// </summary>
    public enum TopicType
    {
        Normal = 0, // 普通
        Vote // 投票帖
    }

    [JsonSerializable(typeof(TopicOverview))]
    public partial class TopicOverviewContext : JsonSerializerContext { }
}
