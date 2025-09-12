using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Sdk.Services.Thread;

namespace Uestc.BBS.Mvvm.Models
{
    public class BrowseSettingModel(BrowseSetting setting) : ObservableObject
    {
        /// <summary>
        /// 过滤设置
        /// </summary>
        public FilterSettingModel Filter { get; init; } = new(setting.Filter);

        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSettingModel Comment { get; init; } = new(setting.Comment);
    }

    /// <summary>
    /// 过滤设置
    /// </summary>
    public class FilterSettingModel(FilterSetting setting) : ObservableObject
    {
        /// <summary>
        /// 是否启用过滤器
        /// </summary>
        public bool IsFilterEnable
        {
            get => setting.IsFilterEnable;
            set =>
                SetProperty(setting.IsFilterEnable, value, setting, (s, e) => s.IsFilterEnable = e);
        }

        /// <summary>
        /// 屏蔽投票
        /// </summary>
        public bool BlockVote
        {
            get => setting.BlockVote;
            set => SetProperty(setting.BlockVote, value, setting, (s, e) => s.BlockVote = e);
        }

        /// <summary>
        /// 屏蔽匿名用户
        /// </summary>
        public bool BlockAnonymousUser
        {
            get => setting.BlockAnonymousUser;
            set =>
                SetProperty(
                    setting.BlockAnonymousUser,
                    value,
                    setting,
                    (s, e) => s.BlockAnonymousUser = e
                );
        }

        /// <summary>
        /// 屏蔽无图帖
        /// </summary>
        public bool BlockNoImage
        {
            get => setting.BlockNoImage;
            set => SetProperty(setting.BlockNoImage, value, setting, (s, e) => s.BlockNoImage = e);
        }

        /// <summary>
        /// 自定义表达式（返回 true 则屏蔽主题）
        /// </summary>
        public string CustomizedExpression
        {
            get => setting.CustomizedExpression;
            set =>
                SetProperty(
                    setting.CustomizedExpression,
                    value,
                    setting,
                    (s, e) => s.CustomizedExpression = e
                );
        }

        public Func<ThreadOverview, bool> CustomizedFilter => setting.CustomizedFilter;

        /// <summary>
        /// 屏蔽板块
        /// </summary>
        public ObservableCollection<Board> BlockedBoards { get; } = [.. setting.BlockedBoards];

        /// <summary>
        /// 屏蔽关键词
        /// </summary>
        public ObservableCollection<string> BlockedKeywords { get; } = [.. setting.BlockedKeywords];
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
