using System.Text.Json.Serialization;
using Uestc.BBS.Core.JsonConverters;

namespace Uestc.BBS.Core.Services.Forum
{
    public class Topic
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonPropertyName("topic_id")]
        public uint Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonConverter(typeof(StringToReplyTypeConverter))]
        public TopicType Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 浏览量
        /// </summary>
        [JsonPropertyName("hits")]
        public uint ViewCount { get; set; }

        /// <summary>
        /// 回复量
        /// </summary>
        [JsonPropertyName("replies")]
        public uint ReplyCount { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [JsonPropertyName("top")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsTop { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("is_favor")]
        public bool IsFavor { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("special")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsSpecial { get; set; }

        /// <summary>
        /// 是否为精华贴
        /// </summary>
        [JsonPropertyName("essence")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsEssence { get; set; }

        /// <summary>
        /// 移动端标识
        /// </summary>
        public string MobileSign { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonPropertyName("create_date")]
        [JsonConverter(typeof(UnixTimestampStringToDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 主题内容
        /// </summary>
        [JsonPropertyName("content")]
        public RichTextContent[] Contents { get; set; } = [];

        /// <summary>
        /// 楼主（1 楼） Pid
        /// </summary>
        [JsonPropertyName("reply_posts_id")]
        public uint ReplyPostsId { get; set; }

        #region 发帖用户

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
        /// 用户性别
        /// </summary>
        [JsonPropertyName("gender")]
        public Gender UserGender { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [JsonPropertyName("icon")]
        public string UserAvatar { get; set; } = string.Empty;

        /// <summary>
        /// 用户等级
        /// </summary>
        [JsonPropertyName("level")]
        public uint UserLevel { get; set; }

        /// <summary>
        /// 用户组
        /// </summary>
        public string UserTitle { get; set; } = string.Empty;

        /// <summary>
        /// 是否关注
        /// </summary>
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsFollow { get; set; }

        #endregion

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
        ///
        /// </summary>
        public uint SortId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public uint Status { get; set; }

        /// <summary>
        ///
        /// </summary>
        public uint ReplyStatus { get; set; }

        [Obsolete("This property is not used in the current version of the API.")]
        public Rate RateList { get; set; } = new();

        public ExtraPanel ExtraPanel { get; set; } = new();
    }

    #region 评分
    public class Rate
    {
        public RateHead Head { get; set; } = new();

        public RateTotal Total { get; set; } = new();

        [JsonPropertyName("body")]
        public RateItem[] Items { get; set; } = [];

        public string ShowAllUrl { get; set; } = string.Empty;
    }

    public class RateTotal
    {
        [JsonPropertyName("field1")]
        public uint UserCount { get; set; }

        [JsonPropertyName("field2")]
        public uint WaterCount { get; set; }
    }

    public class RateHead
    {
        public string Field1 { get; set; } = string.Empty;

        public string Field2 { get; set; } = string.Empty;

        public string Field3 { get; set; } = string.Empty;
    }

    public class RateItem
    {
        public string UserName { get; set; } = string.Empty;

        public int Score { get; set; }

        public string Comment { get; set; } = string.Empty;
    }

    public class Reward { }

    #endregion
}
