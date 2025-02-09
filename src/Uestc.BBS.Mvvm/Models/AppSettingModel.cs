using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class AppSettingModel(AppSetting setting) : ObservableObject
    {
        /// <summary>
        /// 外观设置
        /// </summary>
        public ApperanceSettingModel Apperance { get; init; } = new(setting.Apperance);

        /// <summary>
        /// 授权设置
        /// </summary>
        public AuthSettingModel Auth { get; init; } = new(setting.Auth);

        /// <summary>
        /// 日志服务
        /// </summary>
        public LogSettingModel Log { get; init; } = new(setting.Log);

        /// <summary>
        /// 更新服务
        /// </summary>
        public UpgradeSettingModel Upgrade { get; init; } = new(setting.Upgrade);

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="path"></param>
        public void Save(string? path = null) => setting.Save(path);
    }

    #region 外观

    /// <summary>
    /// 外观设置
    /// </summary>
    public class ApperanceSettingModel(ApperanceSetting apperanceSetting) : ObservableObject
    {
        /// <summary>
        /// 主题
        /// </summary>
        public ThemeColor ThemeColor
        {
            get => apperanceSetting.ThemeColor;
            set =>
                SetProperty(
                    apperanceSetting.ThemeColor,
                    value,
                    apperanceSetting,
                    (setting, theme) => setting.ThemeColor = theme
                );
        }

        /// <summary>
        /// 顶部导航栏是否可见
        /// </summary>
        public bool IsTopNavigateBarEnabled
        {
            get => apperanceSetting.IsTopNavigateBarEnabled;
            set =>
                SetProperty(
                    apperanceSetting.IsTopNavigateBarEnabled,
                    value,
                    apperanceSetting,
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
            get => apperanceSetting.OfficialWebsite;
            set =>
                SetProperty(
                    apperanceSetting.OfficialWebsite,
                    value,
                    apperanceSetting,
                    (setting, website) => setting.OfficialWebsite = website
                );
        }
    }

    /// <summary>
    /// 每日一句设置
    /// </summary>
    public class SearchBarSettingModel(SearchBarSetting searchBarSetting) : ObservableObject
    {
        /// <summary>
        /// 启用每日一句
        /// </summary>
        public bool IsDailySentenceEnabled
        {
            get => searchBarSetting.IsDailySentenceEnabled;
            set =>
                SetProperty(
                    searchBarSetting.IsDailySentenceEnabled,
                    value,
                    searchBarSetting,
                    (setting, enabled) => setting.IsDailySentenceEnabled = enabled
                );
        }

        /// <summary>
        /// 每日一句更新周期（秒）
        /// </summary>
        public uint DailySentenceUpdateTimeInterval
        {
            get => searchBarSetting.DailySentenceUpdateTimeInterval;
            set =>
                SetProperty(
                    searchBarSetting.DailySentenceUpdateTimeInterval,
                    value,
                    searchBarSetting,
                    (setting, interval) => setting.DailySentenceUpdateTimeInterval = interval
                );
        }

        /// <summary>
        /// 搜索历史
        /// </summary>
        public bool IsSearchHistoryEnabled
        {
            get => searchBarSetting.IsSearchHistoryEnabled;
            set =>
                SetProperty(
                    searchBarSetting.IsSearchHistoryEnabled,
                    value,
                    searchBarSetting,
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
        /// <summary>
        /// 静默启动
        /// </summary>
        public bool SlientStart
        {
            get => startupAndShutdownSetting.SlientStart;
            set =>
                SetProperty(
                    startupAndShutdownSetting.SlientStart,
                    value,
                    startupAndShutdownSetting,
                    (setting, enabled) => setting.SlientStart = enabled
                );
        }

        /// <summary>
        /// 开机自启动
        /// </summary>
        public bool StartupOnLaunch
        {
            get => startupAndShutdownSetting.StartupOnLaunch;
            set =>
                SetProperty(
                    startupAndShutdownSetting.StartupOnLaunch,
                    value,
                    startupAndShutdownSetting,
                    (setting, enabled) => setting.StartupOnLaunch = enabled
                );
        }

        /// <summary>
        /// 固定窗口
        /// </summary>
        public bool IsWindowPinned
        {
            get => startupAndShutdownSetting.IsWindowPinned;
            set =>
                SetProperty(
                    startupAndShutdownSetting.IsWindowPinned,
                    value,
                    startupAndShutdownSetting,
                    (setting, pinned) => setting.IsWindowPinned = pinned
                );
        }

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        public WindowCloseBehavior WindowCloseBehavior
        {
            get => startupAndShutdownSetting.WindowCloseBehavior;
            set =>
                SetProperty(
                    startupAndShutdownSetting.WindowCloseBehavior,
                    value,
                    startupAndShutdownSetting,
                    (setting, behavior) => setting.WindowCloseBehavior = behavior
                );
        }
    }

    /// <summary>
    /// 浏览设置
    /// </summary>
    public class BrowsingSettingModel(BrowsingSetting browsingSetting) : ObservableObject
    {
        /// <summary>
        /// 高亮热门主题
        /// </summary>
        public bool HighlightHotTopic
        {
            get => browsingSetting.HighlightHotTopic;
            set =>
                SetProperty(
                    browsingSetting.HighlightHotTopic,
                    value,
                    browsingSetting,
                    (setting, highlight) => setting.HighlightHotTopic = highlight
                );
        }

        /// <summary>
        /// 热门主题阈值
        /// </summary>
        public uint TopicHotThreshold
        {
            get => browsingSetting.TopicHotThreshold;
            set =>
                SetProperty(
                    browsingSetting.TopicHotThreshold,
                    value,
                    browsingSetting,
                    (setting, threshold) => setting.TopicHotThreshold = threshold
                );
        }

        /// <summary>
        /// 主题热度指数加权方案
        /// </summary>
        public TopicHotWeightingScheme TopicHotIndexWeightingScheme
        {
            get => browsingSetting.TopicHotIndexWeightingScheme;
            set =>
                SetProperty(
                    browsingSetting.TopicHotIndexWeightingScheme,
                    value,
                    browsingSetting,
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
    public class CommentSettingModel(CommentSetting commentSetting) : ObservableObject
    {
        /// <summary>
        /// 楼中楼
        /// </summary>
        public bool IsNested
        {
            get => commentSetting.IsNested;
            set =>
                SetProperty(
                    commentSetting.IsNested,
                    value,
                    commentSetting,
                    (setting, nested) => setting.IsNested = nested
                );
        }

        /// <summary>
        /// 强制置顶（置顶评论将显示在热评上方）
        /// </summary>
        public bool ForcedPinned
        {
            get => commentSetting.ForcedPinned;
            set =>
                SetProperty(
                    commentSetting.ForcedPinned,
                    value,
                    commentSetting,
                    (setting, pinned) => setting.ForcedPinned = pinned
                );
        }

        /// <summary>
        /// 热评点赞阈值
        /// </summary>
        public uint HotCommentLikesThreshold
        {
            get => commentSetting.HotCommentLikesThreshold;
            set =>
                SetProperty(
                    commentSetting.HotCommentLikesThreshold,
                    value,
                    commentSetting,
                    (setting, threshold) => setting.HotCommentLikesThreshold = threshold
                );
        }

        #region 评论区显示内容
        /// <summary>
        /// 评论楼层是否可见
        /// </summary>
        public bool IsCommentFloorVisible
        {
            get => commentSetting.IsCommentFloorVisible;
            set =>
                SetProperty(
                    commentSetting.IsCommentFloorVisible,
                    value,
                    commentSetting,
                    (setting, visible) => setting.IsCommentFloorVisible = visible
                );
        }

        /// <summary>
        /// 用户等级是否可见
        /// </summary>
        public bool IsUserLevelVisible
        {
            get => commentSetting.IsUserLevelVisible;
            set =>
                SetProperty(
                    commentSetting.IsUserLevelVisible,
                    value,
                    commentSetting,
                    (setting, visible) => setting.IsUserLevelVisible = visible
                );
        }

        /// <summary>
        /// 用户勋章是否可见
        /// </summary>
        public bool IsUserBadgeVisible
        {
            get => commentSetting.IsUserBadgeVisible;
            set =>
                SetProperty(
                    commentSetting.IsUserBadgeVisible,
                    value,
                    commentSetting,
                    (setting, visible) => setting.IsUserBadgeVisible = visible
                );
        }

        /// <summary>
        /// 用户组是否可见
        /// </summary>
        public bool IsUserGroupVisible
        {
            get => commentSetting.IsUserGroupVisible;
            set =>
                SetProperty(
                    commentSetting.IsUserGroupVisible,
                    value,
                    commentSetting,
                    (setting, visible) => setting.IsUserGroupVisible = visible
                );
        }

        /// <summary>
        /// 用户签名是否可见
        /// </summary>
        public bool IsUserSignatureVisible
        {
            get => commentSetting.IsUserSignatureVisible;
            set =>
                SetProperty(
                    commentSetting.IsUserSignatureVisible,
                    value,
                    commentSetting,
                    (setting, visible) => setting.IsUserSignatureVisible = visible
                );
        }
        #endregion
    }

    #endregion

    #region 授权
    /// <summary>
    /// 授权设置
    /// </summary>
    public class AuthSettingModel(AuthSetting authSetting) : ObservableObject
    {
        /// <summary>
        /// 自动登录
        /// </summary>
        public bool AutoLogin
        {
            get => authSetting.AutoLogin;
            set =>
                SetProperty(
                    authSetting.AutoLogin,
                    value,
                    authSetting,
                    (setting, autoLogin) => setting.AutoLogin = autoLogin
                );
        }

        /// <summary>
        /// 记住密码（取消记住密码仍然会保存密钥信息）
        /// </summary>
        public bool RememberPassword
        {
            get => authSetting.RememberPassword;
            set =>
                SetProperty(
                    authSetting.RememberPassword,
                    value,
                    authSetting,
                    (setting, rememberPassword) => setting.RememberPassword = rememberPassword
                );
        }

        /// <summary>
        /// 默认授权信息 Uid
        /// </summary>
        public uint DefaultCredentialUid
        {
            get => authSetting.DefaultCredentialUid;
            set =>
                SetProperty(
                    authSetting.DefaultCredentialUid,
                    value,
                    authSetting,
                    (setting, defaultCredentialUid) =>
                        setting.DefaultCredentialUid = defaultCredentialUid
                );
        }

        /// <summary>
        /// 默认授权信息
        /// </summary>
        public AuthCredential? DefaultCredential => authSetting.DefaultCredential;

        /// <summary>
        /// 用戶是否授权
        /// </summary>
        public bool IsUserAuthed => authSetting.IsUserAuthed;

        /// <summary>
        /// 授权信息列表（保存本地所有授权信息）
        /// </summary>
        public ObservableCollection<AuthCredential> Credentials { get; set; } =
            [.. authSetting.Credentials];
    }
    #endregion

    public class LogSettingModel(LogSetting logSetting) : ObservableObject
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool IsEnable
        {
            get => logSetting.IsEnable;
            set =>
                SetProperty(
                    logSetting.IsEnable,
                    value,
                    logSetting,
                    (setting, isEnabled) => setting.IsEnable = isEnabled
                );
        }

        /// <summary>
        /// 最低日志级别
        /// </summary>
        public LogLevel MinLevel
        {
            get => logSetting.MinLevel;
            set =>
                SetProperty(
                    logSetting.MinLevel,
                    value,
                    logSetting,
                    (setting, minLevel) => logSetting.MinLevel = minLevel
                );
        }

        /// <summary>
        /// 输出格式
        /// </summary>
        public string OutputFormat
        {
            get => logSetting.OutputFormat;
            set =>
                SetProperty(
                    logSetting.OutputFormat,
                    value,
                    logSetting,
                    (logSetting, format) => logSetting.OutputFormat = format
                );
        }

        /// <summary>
        /// 日志存档大小
        /// </summary>
        public long ArchiveAboveSize
        {
            get => logSetting.ArchiveAboveSize;
            set =>
                SetProperty(
                    logSetting.ArchiveAboveSize,
                    value,
                    logSetting,
                    (setting, size) => setting.ArchiveAboveSize = size
                );
        }

        /// <summary>
        /// 最大日志文件数
        /// </summary>
        public int MaxArchiveFiles
        {
            get => logSetting.MaxArchiveFiles;
            set =>
                SetProperty(
                    logSetting.MaxArchiveFiles,
                    value,
                    logSetting,
                    (setting, count) => setting.MaxArchiveFiles = count
                );
        }
    }

    public class UpgradeSettingModel(UpgradeSetting upgradeSetting) : ObservableObject
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool AcceptBetaVersion
        {
            get => upgradeSetting.AcceptBetaVersion;
            set =>
                SetProperty(
                    upgradeSetting.AcceptBetaVersion,
                    value,
                    upgradeSetting,
                    (setting, accept) => setting.AcceptBetaVersion = accept
                );
        }

        /// <summary>
        /// 上次更新检查时间
        /// </summary>
        public DateTime LastCheckTime
        {
            get => upgradeSetting.LastCheckTime;
            set =>
                SetProperty(
                    upgradeSetting.LastCheckTime,
                    value,
                    upgradeSetting,
                    (setting, time) => setting.LastCheckTime = time
                );
        }

        /// <summary>
        /// 更新地址
        /// </summary>
        public string Mirror
        {
            get => upgradeSetting.Mirror;
            set =>
                SetProperty(
                    upgradeSetting.Mirror,
                    value,
                    upgradeSetting,
                    (setting, mirror) => setting.Mirror = mirror
                );
        }
    }
}
