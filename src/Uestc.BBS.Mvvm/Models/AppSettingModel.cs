using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class AppSettingModel(AppSetting setting) : ObservableObject
    {
        private readonly AppSetting _setting = setting;

        public ApperanceSettingModel Apperance { get; init; } =
            new ApperanceSettingModel(setting.Apperance);

        public void Save(string? path = null) => _setting.Save(path);
    }

    /// <summary>
    /// 外观设置
    /// </summary>
    public class ApperanceSettingModel(ApperanceSetting apperanceSetting) : ObservableObject
    {
        private readonly ApperanceSetting _apperanceSetting = apperanceSetting;

        /// <summary>
        /// 主题
        /// </summary>
        public ThemeColor ThemeColor
        {
            get => _apperanceSetting.ThemeColor;
            set =>
                SetProperty(
                    _apperanceSetting.ThemeColor,
                    value,
                    _apperanceSetting,
                    (setting, theme) => setting.ThemeColor = theme
                );
        }

        /// <summary>
        /// 顶部导航栏是否可见
        /// </summary>
        public bool IsTopNavigateBarEnabled
        {
            get => _apperanceSetting.IsTopNavigateBarEnabled;
            set =>
                SetProperty(
                    _apperanceSetting.IsTopNavigateBarEnabled,
                    value,
                    _apperanceSetting,
                    (setting, enabled) => setting.IsTopNavigateBarEnabled = enabled
                );
        }

        /// <summary>
        /// 搜索栏
        /// </summary>
        public SearchBarSettingModel SearchBar { get; set; } = new(apperanceSetting.SearchBar);

        /// <summary>
        /// 启动和关闭设置
        /// </summary>
        public StartupAndShutdownSettingModel StartupAndShutdown { get; set; } =
            new(apperanceSetting.StartupAndShutdown);

        /// <summary>
        /// 浏览设置
        /// </summary>
        public BrowsingSettingModel Browsing { get; set; } = new(apperanceSetting.Browsing);

        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSettingModel Comment { get; set; } = new(apperanceSetting.Comment);

        /// <summary>
        /// 侧边栏菜单列表
        /// </summary>
        public MenuItem[] MenuItems { get; set; } =
            [
                new MenuItem
                {
                    Key = "Home",
                    Name = "主 页",
                    Symbol = "Home",
                    IsActive = true,
                    DockTop = true,
                },
                new MenuItem
                {
                    Key = "Sections",
                    Name = "版 块",
                    Symbol = "Apps",
                    IsActive = false,
                    DockTop = true,
                },
                new MenuItem
                {
                    Key = "Services",
                    Name = "服 务",
                    Symbol = "Rocket",
                    IsActive = false,
                    DockTop = true,
                },
                new MenuItem
                {
                    Key = "Moments",
                    Name = "动 态",
                    Symbol = "Scan",
                    IsActive = false,
                    DockTop = true,
                },
                new MenuItem
                {
                    Key = "Post",
                    Name = "发 布",
                    Symbol = "SaveCopy",
                    IsActive = false,
                    DockTop = true,
                },
                new MenuItem
                {
                    Key = "Settings",
                    Name = "设 置",
                    Symbol = "Settings",
                    IsActive = false,
                    DockTop = false,
                },
                new MenuItem
                {
                    Key = "Messages",
                    Name = "消 息",
                    Symbol = "Mail",
                    IsActive = false,
                    DockTop = false,
                },
            ];

        /// <summary>
        /// 首页版块 Tab 栏
        /// </summary>
        public BoardTabItem[] BoardTabItems { get; set; } =
            [
                new()
                {
                    Name = "最新发表",
                    Route = "forum/topiclist",
                    Board = Board.Latest,
                    SortType = TopicSortType.New,
                    PageSize = 15,
                    RequirePreviewSources = true,
                    ModuleId = 0,
                },
                new()
                {
                    Name = "最新回复",
                    Route = "forum/topiclist",
                    Board = Board.Latest,
                    SortType = TopicSortType.All,
                    PageSize = 15,
                    RequirePreviewSources = true,
                    ModuleId = 0,
                },
                new()
                {
                    Name = "热门",
                    Route = "portal/newslist",
                    Board = Board.Anonymous,
                    SortType = TopicSortType.All,
                    PageSize = 15,
                    RequirePreviewSources = true,
                    ModuleId = 2,
                },
                new()
                {
                    Name = "精华",
                    Route = "forum/topiclist",
                    Board = Board.Latest,
                    SortType = TopicSortType.Essence,
                    PageSize = 15,
                    RequirePreviewSources = true,
                    ModuleId = 0,
                },
                new()
                {
                    Name = "淘专辑",
                    Route = "forum/topiclist",
                    Board = Board.ExamiHome,
                    SortType = TopicSortType.New,
                    PageSize = 15,
                    RequirePreviewSources = true,
                    ModuleId = 0,
                },
            ];

        /// <summary>
        /// 官方论坛链接
        /// </summary>
        public string OfficialWebsite
        {
            get => _apperanceSetting.OfficialWebsite;
            set =>
                SetProperty(
                    _apperanceSetting.OfficialWebsite,
                    value,
                    _apperanceSetting,
                    (setting, website) => setting.OfficialWebsite = website
                );
        }
    }

    /// <summary>
    /// 每日一句设置
    /// </summary>
    public class SearchBarSettingModel(SearchBarSetting searchBarSetting) : ObservableObject
    {
        private readonly SearchBarSetting _searchBarSetting = searchBarSetting;

        /// <summary>
        /// 启用每日一句
        /// </summary>
        public bool IsDailySentenceEnabled
        {
            get => _searchBarSetting.IsDailySentenceEnabled;
            set =>
                SetProperty(
                    _searchBarSetting.IsDailySentenceEnabled,
                    value,
                    _searchBarSetting,
                    (setting, enabled) => setting.IsDailySentenceEnabled = enabled
                );
        }

        /// <summary>
        /// 每日一句更新周期（秒）
        /// </summary>
        public uint DailySentenceUpdateTimeInterval
        {
            get => _searchBarSetting.DailySentenceUpdateTimeInterval;
            set =>
                SetProperty(
                    _searchBarSetting.DailySentenceUpdateTimeInterval,
                    value,
                    _searchBarSetting,
                    (setting, interval) => setting.DailySentenceUpdateTimeInterval = interval
                );
        }

        /// <summary>
        /// 搜索历史
        /// </summary>
        public bool IsSearchHistoryEnabled
        {
            get => _searchBarSetting.IsSearchHistoryEnabled;
            set =>
                SetProperty(
                    _searchBarSetting.IsSearchHistoryEnabled,
                    value,
                    _searchBarSetting,
                    (setting, enabled) => setting.IsSearchHistoryEnabled = enabled
                );
        }
    }

    /// <summary>
    /// 启动和关闭设置
    /// </summary>
    public class StartupAndShutdownSettingModel(StartupAndShutdownSetting startupAndShutdownSetting)
        : ObservableObject
    {
        private readonly StartupAndShutdownSetting _startupAndShutdownSetting =
            startupAndShutdownSetting;

        /// <summary>
        /// 静默启动
        /// </summary>
        public bool SlientStart
        {
            get => _startupAndShutdownSetting.SlientStart;
            set =>
                SetProperty(
                    _startupAndShutdownSetting.SlientStart,
                    value,
                    _startupAndShutdownSetting,
                    (setting, enabled) => setting.SlientStart = enabled
                );
        }

        /// <summary>
        /// 开机自启动
        /// </summary>
        public bool StartupOnLaunch
        {
            get => _startupAndShutdownSetting.StartupOnLaunch;
            set =>
                SetProperty(
                    _startupAndShutdownSetting.StartupOnLaunch,
                    value,
                    _startupAndShutdownSetting,
                    (setting, enabled) => setting.StartupOnLaunch = enabled
                );
        }

        /// <summary>
        /// 固定窗口
        /// </summary>
        public bool IsWindowPinned
        {
            get => _startupAndShutdownSetting.IsWindowPinned;
            set =>
                SetProperty(
                    _startupAndShutdownSetting.IsWindowPinned,
                    value,
                    _startupAndShutdownSetting,
                    (setting, pinned) => setting.IsWindowPinned = pinned
                );
        }

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        public WindowCloseBehavior WindowCloseBehavior
        {
            get => _startupAndShutdownSetting.WindowCloseBehavior;
            set =>
                SetProperty(
                    _startupAndShutdownSetting.WindowCloseBehavior,
                    value,
                    _startupAndShutdownSetting,
                    (setting, behavior) => setting.WindowCloseBehavior = behavior
                );
        }
    }

    /// <summary>
    /// 浏览设置
    /// </summary>
    public class BrowsingSettingModel(BrowsingSetting browsingSetting) : ObservableObject
    {
        private readonly BrowsingSetting _browsingSetting = browsingSetting;

        /// <summary>
        /// 高亮热门主题
        /// </summary>
        public bool HighlightHotTopic
        {
            get => _browsingSetting.HighlightHotTopic;
            set =>
                SetProperty(
                    _browsingSetting.HighlightHotTopic,
                    value,
                    _browsingSetting,
                    (setting, highlight) => setting.HighlightHotTopic = highlight
                );
        }

        /// <summary>
        /// 热门主题阈值
        /// </summary>
        public uint TopicHotThreshold
        {
            get => _browsingSetting.TopicHotThreshold;
            set =>
                SetProperty(
                    _browsingSetting.TopicHotThreshold,
                    value,
                    _browsingSetting,
                    (setting, threshold) => setting.TopicHotThreshold = threshold
                );
        }

        /// <summary>
        /// 主题热度指数加权方案
        /// </summary>
        public TopicHotWeightingScheme TopicHotIndexWeightingScheme
        {
            get => _browsingSetting.TopicHotIndexWeightingScheme;
            set =>
                SetProperty(
                    _browsingSetting.TopicHotIndexWeightingScheme,
                    value,
                    _browsingSetting,
                    (setting, scheme) => setting.TopicHotIndexWeightingScheme = scheme
                );
        }
    }

    /// <summary>
    /// 主题热度指数加权方案
    /// </summary>
    public class TopicHotWeightingSchemeModel(TopicHotWeightingScheme topicHotWeightingScheme)
        : ObservableObject
    {
        private readonly TopicHotWeightingScheme _topicHotWeightingScheme = topicHotWeightingScheme;

        /// <summary>
        /// 浏览量系数
        /// </summary>
        public uint ViewsCoefficient
        {
            get => _topicHotWeightingScheme.ViewsCoefficient;
            set =>
                SetProperty(
                    _topicHotWeightingScheme.ViewsCoefficient,
                    value,
                    _topicHotWeightingScheme,
                    (scheme, coefficient) => scheme.ViewsCoefficient = coefficient
                );
        }

        /// <summary>
        /// 回复系数
        /// </summary>
        public uint RepliesCoefficient
        {
            get => _topicHotWeightingScheme.RepliesCoefficient;
            set =>
                SetProperty(
                    _topicHotWeightingScheme.RepliesCoefficient,
                    value,
                    _topicHotWeightingScheme,
                    (scheme, coefficient) => scheme.RepliesCoefficient = coefficient
                );
        }

        /// <summary>
        /// 点赞系数
        /// </summary>
        public uint LikesCoefficient
        {
            get => _topicHotWeightingScheme.LikesCoefficient;
            set =>
                SetProperty(
                    _topicHotWeightingScheme.LikesCoefficient,
                    value,
                    _topicHotWeightingScheme,
                    (scheme, coefficient) => scheme.LikesCoefficient = coefficient
                );
        }
    }

    /// <summary>
    /// 评论设置
    /// </summary>
    public class CommentSettingModel(CommentSetting commentSetting) : ObservableObject
    {
        private readonly CommentSetting _commentSetting = commentSetting;

        /// <summary>
        /// 楼中楼
        /// </summary>
        public bool IsNested
        {
            get => _commentSetting.IsNested;
            set =>
                SetProperty(
                    _commentSetting.IsNested,
                    value,
                    _commentSetting,
                    (setting, nested) => setting.IsNested = nested
                );
        }

        /// <summary>
        /// 强制置顶（置顶评论将显示在热评上方）
        /// </summary>
        public bool ForcedPinned
        {
            get => _commentSetting.ForcedPinned;
            set =>
                SetProperty(
                    _commentSetting.ForcedPinned,
                    value,
                    _commentSetting,
                    (setting, pinned) => setting.ForcedPinned = pinned
                );
        }

        /// <summary>
        /// 热评点赞阈值
        /// </summary>
        public uint HotCommentLikesThreshold
        {
            get => _commentSetting.HotCommentLikesThreshold;
            set =>
                SetProperty(
                    _commentSetting.HotCommentLikesThreshold,
                    value,
                    _commentSetting,
                    (setting, threshold) => setting.HotCommentLikesThreshold = threshold
                );
        }

        #region 评论区显示内容
        /// <summary>
        /// 评论楼层是否可见
        /// </summary>
        public bool IsCommentFloorVisible
        {
            get => _commentSetting.IsCommentFloorVisible;
            set =>
                SetProperty(
                    _commentSetting.IsCommentFloorVisible,
                    value,
                    _commentSetting,
                    (setting, visible) => setting.IsCommentFloorVisible = visible
                );
        }

        /// <summary>
        /// 用户等级是否可见
        /// </summary>
        public bool IsUserLevelVisible
        {
            get => _commentSetting.IsUserLevelVisible;
            set =>
                SetProperty(
                    _commentSetting.IsUserLevelVisible,
                    value,
                    _commentSetting,
                    (setting, visible) => setting.IsUserLevelVisible = visible
                );
        }

        /// <summary>
        /// 用户勋章是否可见
        /// </summary>
        public bool IsUserBadgeVisible
        {
            get => _commentSetting.IsUserBadgeVisible;
            set =>
                SetProperty(
                    _commentSetting.IsUserBadgeVisible,
                    value,
                    _commentSetting,
                    (setting, visible) => setting.IsUserBadgeVisible = visible
                );
        }

        /// <summary>
        /// 用户组是否可见
        /// </summary>
        public bool IsUserGroupVisible
        {
            get => _commentSetting.IsUserGroupVisible;
            set =>
                SetProperty(
                    _commentSetting.IsUserGroupVisible,
                    value,
                    _commentSetting,
                    (setting, visible) => setting.IsUserGroupVisible = visible
                );
        }

        /// <summary>
        /// 用户签名是否可见
        /// </summary>
        public bool IsUserSignatureVisible
        {
            get => _commentSetting.IsUserSignatureVisible;
            set =>
                SetProperty(
                    _commentSetting.IsUserSignatureVisible,
                    value,
                    _commentSetting,
                    (setting, visible) => setting.IsUserSignatureVisible = visible
                );
        }
        #endregion
    }
}
