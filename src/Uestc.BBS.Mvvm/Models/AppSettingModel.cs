using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class AppSettingModel(AppSetting setting) : ObservableObject
    {
        /// <summary>
        /// 应用设置
        /// </summary>
        public AppSetting AppSetting => setting;

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
    public class ApperanceSettingModel : ObservableObject
    {
        private readonly ApperanceSetting _apperanceSetting;

        public ApperanceSettingModel(ApperanceSetting apperanceSetting)
        {
            _apperanceSetting = apperanceSetting;

            SearchBar = new SearchBarSettingModel(apperanceSetting.SearchBar);
            StartupAndShutdown = new StartupAndShutdownSettingModel(
                apperanceSetting.StartupAndShutdown
            );
            Browsing = new BrowsingSettingModel(apperanceSetting.Browsing);
            Comment = new CommentSettingModel(apperanceSetting.Comment);
            MenuItems = [.. apperanceSetting.MenuItems.Select(item => new MenuItemModel(item))];
            BoardTabItems =
            [
                .. apperanceSetting.BoardTabItems.Select(item => new BoardTabItemModel(item)),
            ];

            MenuItems.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        apperanceSetting.MenuItems.AddRange(
                            args.NewItems!.Cast<MenuItemModel>().Select(item => item.MenuItem)
                        );
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (
                            var item in args.OldItems!.Cast<MenuItemModel>()
                                .Select(item => item.MenuItem)
                        )
                        {
                            apperanceSetting.MenuItems.Remove(item);
                        }
                        break;
                    default:
                        throw new ArgumentException(
                            "Unhandled collection change action.",
                            nameof(args)
                        );
                }
            };

            BoardTabItems.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        apperanceSetting.BoardTabItems.AddRange(
                            args.NewItems!.Cast<BoardTabItemModel>()
                                .Select(item => item.BoardTabItem)
                        );
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (
                            var item in args.OldItems!.Cast<BoardTabItemModel>()
                                .Select(item => item.BoardTabItem)
                        )
                        {
                            apperanceSetting.BoardTabItems.Remove(item);
                        }
                        break;
                    default:
                        throw new ArgumentException(
                            "Unhandled collection change action.",
                            nameof(args)
                        );
                }
            };
        }

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
        public SearchBarSettingModel SearchBar { get; init; }

        /// <summary>
        /// 启动和关闭设置
        /// </summary>
        public StartupAndShutdownSettingModel StartupAndShutdown { get; init; }

        /// <summary>
        /// 浏览设置
        /// </summary>
        public BrowsingSettingModel Browsing { get; init; }

        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSettingModel Comment { get; init; }

        /// <summary>
        /// 侧边栏菜单列表
        /// </summary>
        public ObservableCollection<MenuItemModel> MenuItems { get; init; }

        /// <summary>
        /// 首页版块 Tab 栏
        /// </summary>
        public ObservableCollection<BoardTabItemModel> BoardTabItems { get; init; }

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

    public class MenuItemModel(MenuItem menuItem) : ObservableObject
    {
        public MenuItem MenuItem => menuItem;

        public MenuItemKey Key
        {
            get => menuItem.Key;
            set => SetProperty(menuItem.Key, value, menuItem, (item, key) => item.Key = key);
        }

        public string Name
        {
            get => menuItem.Name;
            set => SetProperty(menuItem.Name, value, menuItem, (item, name) => item.Name = name);
        }

        public string Symbol
        {
            get => menuItem.Symbol;
            set =>
                SetProperty(
                    menuItem.Symbol,
                    value,
                    menuItem,
                    (item, symbol) => item.Symbol = symbol
                );
        }

        public string Glyph
        {
            get => menuItem.Glyph;
            set =>
                SetProperty(menuItem.Glyph, value, menuItem, (item, glyph) => item.Glyph = glyph);
        }

        public Position Position
        {
            get => menuItem.Position;
            set =>
                SetProperty(
                    menuItem.Position,
                    value,
                    menuItem,
                    (item, position) => item.Position = position
                );
        }
    }

    public class BoardTabItemModel(BoardTabItem boardTabItem) : ObservableObject
    {
        public BoardTabItem BoardTabItem => boardTabItem;

        public string Name
        {
            get => boardTabItem.Name;
            set =>
                SetProperty(
                    boardTabItem.Name,
                    value,
                    boardTabItem,
                    (item, name) => item.Name = name
                );
        }

        public string Route
        {
            get => boardTabItem.Route;
            set =>
                SetProperty(
                    boardTabItem.Route,
                    value,
                    boardTabItem,
                    (item, route) => item.Route = route
                );
        }

        public Board Board
        {
            get => boardTabItem.Board;
            set =>
                SetProperty(
                    boardTabItem.Board,
                    value,
                    boardTabItem,
                    (item, board) => item.Board = board
                );
        }

        public TopicSortType SortType
        {
            get => boardTabItem.SortType;
            set =>
                SetProperty(
                    boardTabItem.SortType,
                    value,
                    boardTabItem,
                    (item, sortType) => item.SortType = sortType
                );
        }

        public int PageSize
        {
            get => boardTabItem.PageSize;
            set =>
                SetProperty(
                    boardTabItem.PageSize,
                    value,
                    boardTabItem,
                    (item, pageSize) => item.PageSize = pageSize
                );
        }

        public bool RequirePreviewSources
        {
            get => boardTabItem.RequirePreviewSources;
            set =>
                SetProperty(
                    boardTabItem.RequirePreviewSources,
                    value,
                    boardTabItem,
                    (item, require) => item.RequirePreviewSources = require
                );
        }

        public int ModuleId
        {
            get => boardTabItem.ModuleId;
            set =>
                SetProperty(
                    boardTabItem.ModuleId,
                    value,
                    boardTabItem,
                    (item, moduleId) => item.ModuleId = moduleId
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
        public bool SilentStart
        {
            get => startupAndShutdownSetting.SilentStart;
            set =>
                SetProperty(
                    startupAndShutdownSetting.SilentStart,
                    value,
                    startupAndShutdownSetting,
                    (setting, enabled) => setting.SilentStart = enabled
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
    public class AuthSettingModel : ObservableObject
    {
        private readonly AuthSetting _authSetting;

        public AuthSettingModel(AuthSetting authSetting)
        {
            _authSetting = authSetting;

            Credentials = [.. _authSetting.Credentials.Select(c => new AuthCredentialModel(c))];
            Credentials.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        authSetting.Credentials.AddRange(
                            args.NewItems!.Cast<AuthCredentialModel>().Select(c => c.AuthCredential)
                        );
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (
                            var item in args.OldItems!.Cast<AuthCredentialModel>()
                                .Select(c => c.AuthCredential)
                        )
                        {
                            authSetting.Credentials.Remove(item);
                        }
                        break;
                    default:
                        throw new ArgumentException(
                            "Unhandled collection change action.",
                            nameof(args)
                        );
                }
            };
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        public bool AutoLogin
        {
            get => _authSetting.AutoLogin;
            set =>
                SetProperty(
                    _authSetting.AutoLogin,
                    value,
                    _authSetting,
                    (setting, autoLogin) => setting.AutoLogin = autoLogin
                );
        }

        /// <summary>
        /// 记住密码（取消记住密码仍然会保存密钥信息）
        /// </summary>
        public bool RememberPassword
        {
            get => _authSetting.RememberPassword;
            set =>
                SetProperty(
                    _authSetting.RememberPassword,
                    value,
                    _authSetting,
                    (setting, rememberPassword) => setting.RememberPassword = rememberPassword
                );
        }

        /// <summary>
        /// 默认授权信息 Uid
        /// </summary>
        public uint DefaultCredentialUid
        {
            get => _authSetting.DefaultCredentialUid;
            set
            {
                SetProperty(
                    _authSetting.DefaultCredentialUid,
                    value,
                    _authSetting,
                    (setting, defaultCredentialUid) =>
                        setting.DefaultCredentialUid = defaultCredentialUid
                );
                OnPropertyChanged(nameof(IsUserAuthed));
                OnPropertyChanged(nameof(DefaultCredential));
            }
        }

        /// <summary>
        /// 默认授权信息
        /// </summary>
        public AuthCredentialModel? DefaultCredential =>
            Credentials.FirstOrDefault(c => c.Uid == DefaultCredentialUid);

        /// <summary>
        /// 用戶是否授权
        /// </summary>
        public bool IsUserAuthed => _authSetting.IsUserAuthed;

        /// <summary>
        /// 授权信息列表（保存本地所有授权信息）
        /// </summary>
        public ObservableCollection<AuthCredentialModel> Credentials { get; init; }
    }

    public class AuthCredentialModel(AuthCredential authCredential) : ObservableObject
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthCredential AuthCredential => authCredential;

        /// <summary>
        /// Uid
        /// </summary>
        public uint Uid
        {
            get => authCredential.Uid;
            set =>
                SetProperty(
                    authCredential.Uid,
                    value,
                    authCredential,
                    (credential, uid) => credential.Uid = uid
                );
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get => authCredential.Name;
            set =>
                SetProperty(
                    authCredential.Name,
                    value,
                    authCredential,
                    (credential, name) => credential.Name = name
                );
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => authCredential.Password;
            set =>
                SetProperty(
                    authCredential.Password,
                    value,
                    authCredential,
                    (credential, password) => credential.Password = password
                );
        }

        /// <summary>
        /// Token
        /// </summary>
        public string Token
        {
            get => authCredential.Token;
            set =>
                SetProperty(
                    authCredential.Token,
                    value,
                    authCredential,
                    (credential, token) => credential.Token = token
                );
        }

        /// <summary>
        /// Secret
        /// </summary>
        public string Secret
        {
            get => authCredential.Secret;
            set =>
                SetProperty(
                    authCredential.Secret,
                    value,
                    authCredential,
                    (credential, secert) => credential.Secret = secert
                );
        }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar
        {
            get => authCredential.Avatar;
            set =>
                SetProperty(
                    authCredential.Avatar,
                    value,
                    authCredential,
                    (credential, avatar) => credential.Avatar = avatar
                );
        }

        /// <summary>
        /// 用户等级
        /// </summary>
        public uint Level
        {
            get => authCredential.Level;
            set =>
                SetProperty(
                    authCredential.Level,
                    value,
                    authCredential,
                    (credential, level) => credential.Level = level
                );
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public string Group
        {
            get => authCredential.Group;
            set =>
                SetProperty(
                    authCredential.Group,
                    value,
                    authCredential,
                    (credential, group) => credential.Group = group
                );
        }

        /// <summary>
        /// 用户签名
        /// </summary>
        public string Signature
        {
            get => authCredential.Signature;
            set =>
                SetProperty(
                    authCredential.Signature,
                    value,
                    authCredential,
                    (credential, signature) => credential.Signature = signature
                );
        }
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
