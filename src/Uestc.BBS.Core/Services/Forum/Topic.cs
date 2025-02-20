using System.Text.Json.Serialization;
using FastEnumUtility;
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
        /// 楼主（1 楼） pid
        /// </summary>
        [JsonPropertyName("reply_posts_id")]
        public uint ReplyPostsId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 主题类型
        /// </summary>
        [JsonConverter(typeof(StringToReplyTypeConverter))]
        public TopicType Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("special")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsSpecial { get; set; }

        /// <summary>
        ///
        /// </summary>
        public uint SortId { get; set; }

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

        /// <summary>
        /// 回复数量
        /// </summary>
        public uint Replies { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        [JsonPropertyName("hits")]
        public uint Views { get; set; }

        /// <summary>
        /// 是否为精华贴
        /// </summary>
        [JsonPropertyName("essence")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsEssence { get; set; }

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
        /// 创建时间
        /// </summary>
        [JsonPropertyName("create_date")]
        [JsonConverter(typeof(UnixTimestampStringToDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 移动端标识
        /// </summary>
        public string MobileSign { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public uint Status { get; set; }

        public uint ReplyStatus { get; set; }
    }

    public class Reply
    {
        #region 用户

        /// <summary>
        /// 用户 ID
        /// </summary>
        [JsonPropertyName("reply_id")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonPropertyName("reply_name")]
        public string UserNickName { get; set; } = string.Empty;

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
        /// 移动端标识
        /// </summary>
        public string MobileSign { get; set; } = string.Empty;

        #endregion

        #region 回复内容

        /// <summary>
        /// 回复 ID
        /// </summary>
        [JsonPropertyName("reply_posts_id")]
        public uint Id { get; set; }

        [JsonPropertyName("posts_date")]
        [JsonConverter(typeof(UnixTimestampStringToDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        public uint Position { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [JsonPropertyName("reply_content")]
        public RichTextContent[] Contents { get; set; } = [];

        /// <summary>
        /// 回复类型
        /// </summary>
        [JsonPropertyName("reply_type")]
        [JsonConverter(typeof(StringToReplyTypeConverter))]
        public ReplyType Type { get; set; } = ReplyType.Normal;
        #endregion

        #region 引用

        /// <summary>
        /// 是否引用
        /// </summary>
        [JsonPropertyName("is_quote")]
        [JsonConverter(typeof(UintToBoolConverter))]
        public bool IsQuote { get; set; }

        /// <summary>
        /// 被引用回复 ID
        /// </summary>
        [JsonPropertyName("quote_pid")]
        public uint QuoteId { get; set; }

        /// <summary>
        /// 被引用楼层发表时间
        /// </summary>
        [JsonPropertyName("quote_time")]
        [JsonConverter(typeof(UnixTimestampStringToDateTimeConverter))]
        public DateTime QuoteCreatedAt { get; set; }

        /// <summary>
        /// 被引用用户昵称
        /// </summary>
        [JsonPropertyName("quote_user_name")]
        public string QuoteUserName { get; set; } = string.Empty;

        /// <summary>
        /// 被引用回复内容
        /// </summary>
        [JsonPropertyName("quote_content_bare")]
        public string QuoteContent { get; set; } = string.Empty;

        #endregion
    }

    public enum ReplyType
    {
        Normal,
    }

    public class ExtraPanel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        public ExtarPanelType Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Action { get; set; } = string.Empty;

        public string RecommendAdd { get; set; } = string.Empty;
    }

    public enum ExtarPanelType
    {
        [Label("支持")]
        Support
    }
}
