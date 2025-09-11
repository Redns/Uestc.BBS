using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Uestc.BBS.Sdk.Services.Thread;

namespace Uestc.BBS.Core.Models
{
    /// <summary>
    /// 浏览设置
    /// </summary>
    public class BrowseSetting
    {
        /// <summary>
        /// 过滤设置
        /// </summary>
        public FilterSetting Filter { get; set; } = new();

        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSetting Comment { get; set; } = new();
    }

    /// <summary>
    /// 过滤设置
    /// </summary>
    public class FilterSetting
    {
        /// <summary>
        /// 是否启用过滤器
        /// </summary>
        public bool IsFilterEnable { get; set; } = true;

        /// <summary>
        /// 屏蔽投票
        /// </summary>
        public bool BlockVote { get; set; } = false;

        /// <summary>
        /// 屏蔽匿名用户
        /// TODO 考虑使用黑名单实现
        /// </summary>
        public bool BlockAnonymousUser { get; set; } = false;

        /// <summary>
        /// 屏蔽无图帖
        /// </summary>
        public bool BlockNoImage { get; set; } = false;

        /// <summary>
        /// 自定义表达式（返回 true 时屏蔽主题）
        /// </summary>
        public string CustomizedExpression
        {
            get;
            set
            {
                if (field == value)
                {
                    return;
                }
                field = value;

                // 为空时不启用自定义过滤器
                if (string.IsNullOrEmpty(field))
                {
                    CustomizedFilter = t => false;
                    return;
                }

                // 自定义过滤器
                var param = Expression.Parameter(typeof(ThreadOverview), "t");
                var expression = DynamicExpressionParser.ParseLambda([param], typeof(bool), field);
                if (expression.Compile() is not Func<ThreadOverview, bool> customizedFilter)
                {
                    return;
                }
                CustomizedFilter = customizedFilter;
            }
        } = string.Empty;

        /// <summary>
        /// 自定义过滤器
        /// </summary>
        [JsonIgnore]
        public Func<ThreadOverview, bool> CustomizedFilter { get; private set; } = t => false;

        /// <summary>
        /// 屏蔽板块
        /// </summary>
        public List<Board> BlockedBoards { get; set; } = [];

        /// <summary>
        /// 屏蔽关键词
        /// </summary>
        public List<string> BlockedKeywords { get; set; } = [];
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
}
