using System.Text.Json.Serialization;
using PanoramicData.NCalcExtensions;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.User;

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

                // 为空时不启用自定义过滤器
                if (string.IsNullOrEmpty(value))
                {
                    field = value;
                    CustomizedFilter = t => false;
                    return;
                }

                // 自定义过滤器
                var expression = new ExtendedExpression(value);
                if (expression.HasErrors())
                {
                    return;
                }
                // 添加参数
                // TODO 使用源码生成器代替
                expression.Parameters.Add("Id", typeof(uint));
                expression.Parameters.Add("Title", typeof(string));
                expression.Parameters.Add("Board", typeof(Board));
                expression.Parameters.Add("Subject", typeof(string));
                expression.Parameters.Add("DateTime", typeof(DateTime));
                expression.Parameters.Add("BoardName", typeof(string));
                expression.Parameters.Add("PreviewImageSources", typeof(string[]));

                expression.Parameters.Add("Uid", typeof(uint));
                expression.Parameters.Add("Username", typeof(string));
                expression.Parameters.Add("UserGender", typeof(Gender));
                expression.Parameters.Add("UserAvatar", typeof(string));

                expression.Parameters.Add("ViewCount", typeof(uint));
                expression.Parameters.Add("LikeCount", typeof(uint));
                expression.Parameters.Add("DislikeCount", typeof(uint));
                expression.Parameters.Add("ReplyCount", typeof(uint));

                expression.Parameters.Add("IsHot", typeof(bool));
                expression.Parameters.Add("HasVote", typeof(bool));
                expression.Parameters.Add("IsAnonymous", typeof(bool));

                // 编译表达式
                CustomizedFilter = t =>
                {
                    // TODO 使用源码生成器代替
                    expression.Parameters["Id"] = t.Id;
                    expression.Parameters["Title"] = t.Title;
                    expression.Parameters["Board"] = t.Board;
                    expression.Parameters["Subject"] = t.Subject;
                    expression.Parameters["DateTime"] = t.DateTime;
                    expression.Parameters["BoardName"] = t.BoardName;
                    expression.Parameters["PreviewImageSources"] = t.PreviewImageSources;

                    expression.Parameters["Uid"] = t.Uid;
                    expression.Parameters["Username"] = t.Username;
                    expression.Parameters["UserGender"] = t.UserGender;
                    expression.Parameters["UserAvatar"] = t.UserAvatar;

                    expression.Parameters["ViewCount"] = t.ViewCount;
                    expression.Parameters["LikeCount"] = t.LikeCount;
                    expression.Parameters["DislikeCount"] = t.DislikeCount;
                    expression.Parameters["ReplyCount"] = t.ReplyCount;

                    expression.Parameters["IsHot"] = t.IsHot;
                    expression.Parameters["HasVote"] = t.HasVote;
                    expression.Parameters["IsAnonymous"] = t.IsAnonymous;

                    if (expression.Evaluate() is not bool ret)
                    {
                        return false;
                    }
                    return ret;
                };

                field = value;
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
