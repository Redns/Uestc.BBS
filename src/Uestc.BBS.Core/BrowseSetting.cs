namespace Uestc.BBS.Core
{
    /// <summary>
    /// 浏览设置
    /// </summary>
    public class BrowseSetting
    {
        /// <summary>
        /// 高亮热门主题
        /// </summary>
        public bool HighlightHotTopic { get; set; } = true;

        /// <summary>
        /// 热门主题阈值
        /// </summary>
        public uint TopicHotThreshold { get; set; } = 1000;

        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSetting Comment { get; set; } = new();

        /// <summary>
        /// 主题热度指数加权方案
        /// </summary>
        public TopicHotIndexWeightingScheme TopicHotIndexWeightingScheme { get; set; } = new();
    }

    /// <summary>
    /// 评论设置
    /// </summary>
    public class CommentSetting
    {
        /// <summary>
        /// 楼中楼
        /// </summary>
        public bool IsNested { get; set; } = true;

        /// <summary>
        /// 强制置顶（置顶评论将显示在热评上方）
        /// </summary>
        public bool ForcedPinned { get; set; } = true;

        /// <summary>
        /// 热评点赞阈值
        /// </summary>
        public uint HotCommentLikesThreshold { get; set; } = 5;

        #region 评论区显示内容
        /// <summary>
        /// 评论楼层是否可见
        /// </summary>
        public bool IsCommentFloorVisible { get; set; } = true;

        /// <summary>
        /// 用户等级是否可见
        /// </summary>
        public bool IsUserLevelVisible { get; set; } = true;

        /// <summary>
        /// 用户勋章是否可见
        /// </summary>
        public bool IsUserBadgeVisible { get; set; } = true;

        /// <summary>
        /// 用户组是否可见
        /// </summary>
        public bool IsUserGroupVisible { get; set; } = true;

        /// <summary>
        /// 用户签名是否可见
        /// </summary>
        public bool IsUserSignatureVisible { get; set; } = true;
        #endregion
    }

    /// <summary>
    /// 主题热度指数加权方案
    /// </summary>
    public class TopicHotIndexWeightingScheme
    {
        /// <summary>
        /// 浏览量系数
        /// </summary>
        public uint ViewsCoefficient { get; set; } = 1;

        /// <summary>
        /// 回复系数
        /// </summary>
        public uint RepliesCoefficient { get; set; } = 10;

        /// <summary>
        /// 点赞系数
        /// </summary>
        public uint LikesCoefficient { get; set; } = 8;
    }
}
