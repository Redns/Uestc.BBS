using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Mvvm.Models
{
    public class BrowseSettingModel(BrowseSetting setting) : ObservableObject
    {
        /// <summary>
        /// 高亮热门主题
        /// </summary>
        public bool HighlightHotTopic
        {
            get => setting.HighlightHotTopic;
            set =>
                SetProperty(
                    setting.HighlightHotTopic,
                    value,
                    setting,
                    (setting, highlight) => setting.HighlightHotTopic = highlight
                );
        }

        /// <summary>
        /// 热门主题阈值
        /// </summary>
        public uint TopicHotThreshold
        {
            get => setting.TopicHotThreshold;
            set =>
                SetProperty(
                    setting.TopicHotThreshold,
                    value,
                    setting,
                    (setting, threshold) => setting.TopicHotThreshold = threshold
                );
        }

        /// <summary>
        /// 主题热度指数加权方案
        /// </summary>
        public TopicHotIndexWeightingSchemeModel TopicHotIndexWeightingScheme { get; init; } =
            new(setting.TopicHotIndexWeightingScheme);

        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSettingModel Comment { get; init; } = new(setting.Comment);
    }

    /// <summary>
    /// 主题热度指数加权方案
    /// </summary>
    public class TopicHotIndexWeightingSchemeModel(
        TopicHotIndexWeightingScheme topicHotWeightingScheme
    ) : ObservableObject
    {
        /// <summary>
        /// 浏览量系数
        /// </summary>
        public uint ViewsCoefficient
        {
            get => topicHotWeightingScheme.ViewsCoefficient;
            set =>
                SetProperty(
                    topicHotWeightingScheme.ViewsCoefficient,
                    value,
                    topicHotWeightingScheme,
                    (scheme, coefficient) => scheme.ViewsCoefficient = coefficient
                );
        }

        /// <summary>
        /// 回复系数
        /// </summary>
        public uint RepliesCoefficient
        {
            get => topicHotWeightingScheme.RepliesCoefficient;
            set =>
                SetProperty(
                    topicHotWeightingScheme.RepliesCoefficient,
                    value,
                    topicHotWeightingScheme,
                    (scheme, coefficient) => scheme.RepliesCoefficient = coefficient
                );
        }

        /// <summary>
        /// 点赞系数
        /// </summary>
        public uint LikesCoefficient
        {
            get => topicHotWeightingScheme.LikesCoefficient;
            set =>
                SetProperty(
                    topicHotWeightingScheme.LikesCoefficient,
                    value,
                    topicHotWeightingScheme,
                    (scheme, coefficient) => scheme.LikesCoefficient = coefficient
                );
        }
    }

    /// <summary>
    /// 评论设置
    /// </summary>
    public class CommentSettingModel(CommentSetting setting) : ObservableObject
    {
        /// <summary>
        /// 楼中楼
        /// </summary>
        public bool IsNested
        {
            get => setting.IsNested;
            set =>
                SetProperty(
                    setting.IsNested,
                    value,
                    setting,
                    (setting, nested) => setting.IsNested = nested
                );
        }

        /// <summary>
        /// 强制置顶（置顶评论将显示在热评上方）
        /// </summary>
        public bool ForcedPinned
        {
            get => setting.ForcedPinned;
            set =>
                SetProperty(
                    setting.ForcedPinned,
                    value,
                    setting,
                    (setting, pinned) => setting.ForcedPinned = pinned
                );
        }

        /// <summary>
        /// 热评点赞阈值
        /// </summary>
        public uint HotCommentLikesThreshold
        {
            get => setting.HotCommentLikesThreshold;
            set =>
                SetProperty(
                    setting.HotCommentLikesThreshold,
                    value,
                    setting,
                    (setting, threshold) => setting.HotCommentLikesThreshold = threshold
                );
        }

        #region 评论区显示内容
        /// <summary>
        /// 评论楼层是否可见
        /// </summary>
        public bool IsCommentFloorVisible
        {
            get => setting.IsCommentFloorVisible;
            set =>
                SetProperty(
                    setting.IsCommentFloorVisible,
                    value,
                    setting,
                    (setting, visible) => setting.IsCommentFloorVisible = visible
                );
        }

        /// <summary>
        /// 用户等级是否可见
        /// </summary>
        public bool IsUserLevelVisible
        {
            get => setting.IsUserLevelVisible;
            set =>
                SetProperty(
                    setting.IsUserLevelVisible,
                    value,
                    setting,
                    (setting, visible) => setting.IsUserLevelVisible = visible
                );
        }

        /// <summary>
        /// 用户勋章是否可见
        /// </summary>
        public bool IsUserBadgeVisible
        {
            get => setting.IsUserBadgeVisible;
            set =>
                SetProperty(
                    setting.IsUserBadgeVisible,
                    value,
                    setting,
                    (setting, visible) => setting.IsUserBadgeVisible = visible
                );
        }

        /// <summary>
        /// 用户组是否可见
        /// </summary>
        public bool IsUserGroupVisible
        {
            get => setting.IsUserGroupVisible;
            set =>
                SetProperty(
                    setting.IsUserGroupVisible,
                    value,
                    setting,
                    (setting, visible) => setting.IsUserGroupVisible = visible
                );
        }

        /// <summary>
        /// 用户签名是否可见
        /// </summary>
        public bool IsUserSignatureVisible
        {
            get => setting.IsUserSignatureVisible;
            set =>
                SetProperty(
                    setting.IsUserSignatureVisible,
                    value,
                    setting,
                    (setting, visible) => setting.IsUserSignatureVisible = visible
                );
        }
        #endregion
    }
}
