using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;
using FastEnumUtility;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Core
{
    public class AppSetting
    {
        /// <summary>
        /// 外观设置
        /// </summary>
        public ApperanceSetting Apperance { get; set; } = new();

        /// <summary>
        /// 授权设置
        /// </summary>
        public AuthSetting Auth { get; set; } = new();

        /// <summary>
        /// 同步设置
        /// </summary>
        public SyncSetting Sync { get; set; } = new();

        /// <summary>
        /// 日志设置
        /// </summary>
        public LogSetting Log { get; set; } = new();

        /// <summary>
        /// 更新设置
        /// </summary>
        public UpgradeSetting Upgrade { get; set; } = new();

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="secret">配置文件解密密钥</param>
        /// <returns></returns>
        public static AppSetting Load(string? path = null)
        {
            AppSetting? appSetting;

            try
            {
                path ??= Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    AppDomain.CurrentDomain.FriendlyName,
                    "appsettings.json"
                );
                if (File.Exists(path))
                {
                    appSetting = JsonSerializer.Deserialize<AppSetting>(
                        File.ReadAllText(path),
                        SerializerOptions
                    );
                    if (appSetting is not null)
                    {
                        return appSetting;
                    }
                }
            }
            catch (Exception e)
            {
                ServiceExtension
                    .Services.GetRequiredService<ILogService>()
                    .Error(string.Format("Appsetting file {0} load failed", path), e);
            }

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }
            appSetting = new AppSetting();
            appSetting.Save(path);

            return appSetting;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="secret">配置文件加密密钥</param>
        public void Save(string? path = null)
        {
            File.WriteAllText(
                path
                    ?? Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        AppDomain.CurrentDomain.FriendlyName,
                        "appsettings.json"
                    ),
                ToString()
            );
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, SerializerOptions);
        }

        public static readonly JsonSerializerOptions SerializerOptions =
            new()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = JsonTypeInfoResolver.Combine(
                    AppSettingContext.Default,
                    new DefaultJsonTypeInfoResolver()
                ),
            };
    }

    /// <summary>
    /// 外观设置
    /// </summary>
    public class ApperanceSetting
    {
        /// <summary>
        /// 主题
        /// </summary>
        public ThemeColor ThemeColor { get; set; } = ThemeColor.System;

        /// <summary>
        /// 顶部导航栏是否可见
        /// </summary>
        public bool IsTopNavigateBarEnabled { get; set; } = true;

        /// <summary>
        /// 搜索栏
        /// </summary>
        public SearchBarSetting SearchBar { get; set; } = new();

        /// <summary>
        /// 启动和关闭设置
        /// </summary>
        public StartupAndShutdownSetting StartupAndShutdown { get; set; } = new();

        /// <summary>
        /// 浏览设置
        /// </summary>
        public BrowsingSetting Browsing { get; set; } = new();

        /// <summary>
        /// 评论设置
        /// </summary>
        public CommentSetting Comment { get; set; } = new();

        /// <summary>
        /// 侧边栏菜单列表
        /// </summary>
        public List<MenuItem> MenuItems { get; init; } =
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
        public List<BoardTabItem> BoardTabItems { get; init; } =
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
        public string OfficialWebsite { get; set; } = "https://bbs.uestc.edu.cn/new";
    }

    /// <summary>
    /// 每日一句设置
    /// </summary>
    public class SearchBarSetting
    {
        /// <summary>
        /// 启用每日一句
        /// </summary>
        public bool IsDailySentenceEnabled { get; set; } = true;

        /// <summary>
        /// 每日一句更新周期（秒）
        /// </summary>
        public uint DailySentenceUpdateTimeInterval { get; set; } = 60;

        /// <summary>
        /// 搜索历史
        /// </summary>
        public bool IsSearchHistoryEnabled { get; set; } = true;
    }

    /// <summary>
    /// 启动和关闭设置
    /// </summary>
    public class StartupAndShutdownSetting
    {
        /// <summary>
        /// 静默启动
        /// </summary>
        public bool SilentStart { get; set; } = false;

        /// <summary>
        /// 固定窗口
        /// </summary>
        public bool IsWindowPinned { get; set; } = false;

        /// <summary>
        /// 窗口关闭行为
        /// </summary>
        public WindowCloseBehavior WindowCloseBehavior { get; set; } = WindowCloseBehavior.Hide;
    }

    /// <summary>
    /// 浏览设置
    /// </summary>
    public class BrowsingSetting
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
        /// 主题热度指数加权方案
        /// </summary>
        public TopicHotWeightingScheme TopicHotIndexWeightingScheme { get; set; } = new();
    }

    /// <summary>
    /// 主题热度指数加权方案
    /// </summary>
    public class TopicHotWeightingScheme
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
    /// 主题
    /// </summary>
    public enum ThemeColor
    {
        [Label("浅色")]
        Light = 0,

        [Label("深色")]
        Dark,

        [Label("跟随系统")]
        System,
    }

    /// <summary>
    /// 窗口关闭行为
    /// </summary>
    public enum WindowCloseBehavior
    {
        [Label("退出应用")]
        Exit,

        [Label("隐藏窗口")]
        Hide,

        [Label("隐藏窗口 + 效率模式")]
        HideWithEfficiencyMode,
    }

    public enum MenuItemKey
    {
        Home = 0,
        Sections,
        Services,
        Moments,
        Post,
        Messages,
        Settings,
    }

    public class MenuItem
    {
        public string Key { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Symbol { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;

        public bool DockTop { get; set; } = true;
    }

    public class BoardTabItem
    {
        /// <summary>
        /// 板块名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Route { get; set; } = string.Empty;

        /// <summary>
        /// 我也不知道这是什么
        /// </summary>
        public uint ModuleId { get; set; } = 0;

        /// <summary>
        /// 板块编号
        /// </summary>
        public Board Board { get; set; } = Board.Latest;

        /// <summary>
        /// 排序类型
        /// </summary>
        public TopicSortType SortType { get; set; } = TopicSortType.New;

        /// <summary>
        /// 分页大小
        /// </summary>
        public uint PageSize { get; set; } = 15;

        /// <summary>
        /// 获取预览图
        /// </summary>
        public bool RequirePreviewSources { get; set; } = false;
    }

    /// <summary>
    /// 授权设置
    /// </summary>
    public class AuthSetting
    {
        /// <summary>
        /// 自动登录
        /// </summary>
        public bool AutoLogin { get; set; } = false;

        /// <summary>
        /// 记住密码（取消记住密码仍然会保存密钥信息）
        /// </summary>
        public bool RememberPassword { get; set; } = false;

        /// <summary>
        /// 默认授权信息 Uid
        /// </summary>
        public uint DefaultCredentialUid { get; set; }

        /// <summary>
        /// 默认授权信息
        /// </summary>
        [JsonIgnore]
        public AuthCredential? DefaultCredential =>
            Credentials.FirstOrDefault(c => c.Uid == DefaultCredentialUid);

        /// <summary>
        /// 用戶是否授权
        /// </summary>
        [JsonIgnore]
        public bool IsUserAuthed =>
            !string.IsNullOrEmpty(DefaultCredential?.Token)
            && !string.IsNullOrEmpty(DefaultCredential.Secret);

        /// <summary>
        /// 授权信息列表（保存本地所有授权信息）
        /// </summary>
        public List<AuthCredential> Credentials { get; init; } = [];
    }

    /// <summary>
    /// 授权信息
    /// </summary>
    public class AuthCredential
    {
        /// <summary>
        /// 用户唯一识别码
        /// </summary>
        public uint Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 令牌
        /// </summary>

        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 密钥
        /// </summary>

        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// 此处序列化用户 AutoCompleteBox 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// 同步设置
    /// </summary>
    public class SyncSetting
    {
        /// <summary>
        /// 同步模式
        /// </summary>
        public SyncMode Mode { get; set; } = SyncMode.OnStaupAndTiming;

        /// <summary>
        /// 同步密钥
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 最近更新日期
        /// 该日期用于判断是否同步云端设置至本地，在某些情况下用户设备 ID 更改导致本地设置丢失，如果
        /// 该属性默认值为 DateTime.Now，则默认设置将覆盖云端设置
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 更新时间间隔
        /// </summary>
        public TimeSpan TimeInterval { get; set; } = TimeSpan.FromMinutes(60);

        /// <summary>
        /// WebDAV 服务地址
        /// </summary>
        public string Api { get; set; } = "https://dav.jianguoyun.com/dav";

        /// <summary>
        /// WebDAV 服务用户名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// WebDAV 服务密码
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// 同步模式
    /// </summary>
    public enum SyncMode
    {
        [Description("手动")]
        None = 0, // 手动

        [Description("启动时同步")]
        OnStartup, // 启动时同步

        [Description("启动时+定时同步")]
        OnStaupAndTiming // 启动时 + 定时同步
        ,
    }

    public class LogSetting
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// 最低日志级别
        /// </summary>
        public LogLevel MinLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// 输出格式
        /// </summary>
        public string OutputFormat { get; set; } =
            "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [Uestc.BBS ${level}] ${message}${onexception:${newline}${exception:format=toString}${exception:format=StackTrace}}";

        /// <summary>
        /// 日志存档大小（MB）
        /// </summary>
        public long ArchiveAboveSize { get; set; } = 8;

        /// <summary>
        /// 最大日志文件数
        /// </summary>
        public int MaxArchiveFiles { get; set; } = 8;
    }

    public class UpgradeSetting
    {
        /// <summary>
        /// 启用服务
        /// </summary>
        public bool AcceptBetaVersion { get; set; } = false;

        /// <summary>
        /// 上次更新检查时间
        /// </summary>
        public DateTime LastCheckTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 更新地址
        /// </summary>
        public string Mirror { get; set; } = "https://mirrors.krins.cloud/Uestc.BBS";
    }

    [JsonSerializable(typeof(AppSetting))]
    public partial class AppSettingContext : JsonSerializerContext { }
}
