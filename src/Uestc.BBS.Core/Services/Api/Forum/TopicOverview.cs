using System.Text.Json.Serialization;
using FastEnumUtility;
using Uestc.BBS.Core.Helpers;
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
        [JsonPropertyName("board_name")]
        public string BoardName { get; set; } = string.Empty;

        /// <summary>
        /// 帖子 ID
        /// 热门主题请使用 SourceId，其余主题使用 TopicId
        /// </summary>
        [JsonPropertyName("topic_id")]
        public uint TopicId { get; set; }

        [JsonPropertyName("source_id")]
        public uint SourceId { get; set; }

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
        /// 最新日期（Unix 毫秒级时间戳）
        /// 热门帖子为发表时间，其余帖子为最新回复时间
        /// </summary>
        [JsonPropertyName("last_reply_date")]
        [JsonConverter(typeof(UnixTimestampStringToDateTimeConverter))]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 是否包含投票
        /// </summary>
        [JsonPropertyName("vote")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool HasVote { get; set; }

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public uint Hot { get; set; }

        /// <summary>
        /// 是否为热门帖
        /// </summary>
        [JsonIgnore]
        public bool IsHot { get; set; } = false;

        /// <summary>
        /// 浏览量
        /// </summary>
        [JsonPropertyName("hits")]
        public uint Views { get; set; }

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
        /// 摘要（为什么两个字段不能合起来）
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// 预览图链接
        /// </summary>
        [JsonPropertyName("pic_path")]
        public string? PicPath { get; set; }

        /// <summary>
        /// TODO WHAT'S THIS
        /// </summary>
        public string Ratio { get; set; } = string.Empty;

        /// <summary>
        /// 枚举性别类型
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 用户头衔
        /// </summary>
        public string UserTitle { get; set; } = string.Empty;

        /// <summary>
        /// 用户等级
        /// </summary>
        public uint UserLevel => UserTitle.GetUserTitleLevel();

        /// <summary>
        /// 点赞数
        /// </summary>
        [JsonPropertyName("recommendadd")]
        public uint Likes { get; set; }

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

        [JsonIgnore]
        public string PreviewSource => ImageList.FirstOrDefault() ?? StringHelper.WhiteSpace;

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
    }

    /// <summary>
    /// 帖子类型
    /// </summary>
    public enum TopicType
    {
        [Label("普通")]
        Normal = 0,

        [Label("投票")]
        Vote,
    }

    /// <summary>
    /// 板块
    /// </summary>
    public enum Board
    {
        /// <summary>
        /// XXX 热门板块实际上不需要 Board 参数，此处是用于标记热门版块主题
        /// </summary>
        [Label("热门")]
        Hot = -1,

        [Label("最新发表", (int)TopicSortType.New)]
        [Label("最新回复", (int)TopicSortType.All)]
        Latest = 0,

        [Label("水手之家")]
        WaterHome = 25,

        [Label("就业创业")]
        EmploymentAndEntrepreneurship = 174,

        [Label("交通出行")]
        Transportation = 225,

        [Label("密语")]
        Anonymous = 371,

        [Label("考试之家")]
        ExamiHome = 382,
    }

    /// <summary>
    /// 板块子分类
    /// </summary>
    public enum BoardSubcategory
    {
        [Label("全部")]
        All = 0,
    }

    /// <summary>
    /// 帖子排序方式
    /// </summary>
    public enum TopicSortType
    {
        [Label("最新")]
        New = 0,

        [Label("精华")]
        Essence,

        [Label("全部")]
        All,
    }

    /// <summary>
    /// 帖子置顶配置
    /// </summary>
    public enum TopicTopOrder
    {
        [Label("不返回置顶帖")]
        WithoutTop = 0,

        [Label("返回本版置顶帖")]
        WithCurrentSectionTop,

        [Label("返回分类置顶帖")]
        WithCategorySectionTop,

        [Label("返回全局置顶帖")]
        WithGlobalTop,
    }

    public enum Gender
    {
        [Label("未知")]
        Unknown = 0,

        [Label("男")]
        Male,

        [Label("女")]
        Female,
    }

    [JsonSerializable(typeof(TopicOverview))]
    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    )]
    public partial class TopicOverviewContext : JsonSerializerContext { }
}
