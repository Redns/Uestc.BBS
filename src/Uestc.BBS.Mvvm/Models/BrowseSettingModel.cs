using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Mvvm.Models
{
    public class BrowseSettingModel(BrowseSetting setting) : ObservableObject
    {
        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSettingModel Comment { get; init; } = new(setting.Comment);
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
            set => SetProperty(setting.IsNested, value, setting, (s, e) => s.IsNested = e);
        }

        /// <summary>
        /// 强制置顶（置顶评论将显示在热评上方）
        /// </summary>
        public bool ForcedPinned
        {
            get => setting.ForcedPinned;
            set => SetProperty(setting.ForcedPinned, value, setting, (s, e) => s.ForcedPinned = e);
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
                    (s, e) => s.HotCommentLikesThreshold = e
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
                    (s, e) => s.IsCommentFloorVisible = e
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
                    (s, e) => s.IsUserLevelVisible = e
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
                    (s, e) => s.IsUserBadgeVisible = e
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
                    (s, e) => s.IsUserGroupVisible = e
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
                    (s, e) => s.IsUserSignatureVisible = e
                );
        }
        #endregion
    }
}
