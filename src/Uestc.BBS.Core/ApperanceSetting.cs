using FastEnumUtility;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Core
{
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
        /// 侧边栏菜单列表
        /// </summary>
        public List<MenuItem> MenuItems { get; init; } =
            [
                // 侧边栏菜单列表
                new MenuItem
                {
                    Key = MenuItemKey.Home,
                    Name = "主 页",
                    Symbol = "Home",
                    Glyph = "\uE80F",
                    Position = Position.LeftTop,
                },
                new MenuItem
                {
                    Key = MenuItemKey.Sections,
                    Name = "版 块",
                    Symbol = "Apps",
                    Glyph = "\uE74C",
                    Position = Position.LeftTop,
                },
                new MenuItem
                {
                    Key = MenuItemKey.Services,
                    Name = "服 务",
                    Symbol = "Rocket",
                    Glyph = "\uE81E",
                    Position = Position.LeftTop,
                },
                new MenuItem
                {
                    Key = MenuItemKey.Moments,
                    Name = "动 态",
                    Symbol = "Scan",
                    Glyph = "\uE909",
                    Position = Position.LeftTop,
                },
                new MenuItem
                {
                    Key = MenuItemKey.Post,
                    Name = "发 布",
                    Symbol = "SaveCopy",
                    Glyph = "\uECCD",
                    Position = Position.LeftTop,
                },
                new MenuItem
                {
                    Key = MenuItemKey.Messages,
                    Name = "消 息",
                    Symbol = "Mail",
                    Glyph = "\uE715",
                    Position = Position.LeftBottom,
                },
                new MenuItem
                {
                    Key = MenuItemKey.Settings,
                    Name = "设 置",
                    Symbol = "Settings",
                    Glyph = "\uE713",
                    Position = Position.LeftBottom,
                },
                // 顶部导航栏菜单列表
                new MenuItem
                {
                    Key = MenuItemKey.MyFavorites,
                    Name = "我的收藏",
                    Symbol = "StarLineHorizontal3",
                    Glyph = "\uE728",
                    Position = Position.Top,
                },
                new MenuItem
                {
                    Key = MenuItemKey.MyPosts,
                    Name = "我的发表",
                    Symbol = "TextBulletListSquareEdit",
                    Glyph = "\uE932",
                    Position = Position.Top,
                },
                new MenuItem
                {
                    Key = MenuItemKey.MyReplies,
                    Name = "我的回复",
                    Symbol = "Comment",
                    Glyph = "\uE90A",
                    Position = Position.Top,
                },
                new MenuItem
                {
                    Key = MenuItemKey.MyMarks,
                    Name = "我的插眼",
                    Symbol = "EyeShow",
                    Glyph = "\uE7B3",
                    Position = Position.Top,
                },
                new MenuItem
                {
                    Key = MenuItemKey.TopicFilter,
                    Name = "主题过滤",
                    Symbol = "Filter",
                    Glyph = "\uE71C;",
                    Position = Position.Top,
                },
            ];

        /// <summary>
        /// 板块 Tab 栏
        /// </summary>
        public BoardTabSetting BoardTab { get; init; } = new();

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
    /// 板块 Tab 栏设置
    /// </summary>
    public class BoardTabSetting
    {
        /// <summary>
        /// 首页版块 Tab 栏宽度
        /// </summary>
        public double Width { get; set; } = 330;

        /// <summary>
        /// 首页版块 Tab 栏是否启用瀑布流
        /// </summary>
        public bool IsStaggeredLayoutEnabled { get; set; } = false;

        /// <summary>
        /// 首页版块 Tab 栏
        /// </summary>
        public List<BoardTabItem> Items { get; init; } =
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
                    Board = Board.Hot,
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
    }

    /// <summary>
    /// 菜单项
    /// </summary>
    public class MenuItem
    {
        public MenuItemKey Key { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// FluentAvalonia 图标，参考 https://fluenticons.co/outlined
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// egoe MDL2 Assets 字符代码，参考 https://docs.microsoft.com/zh-cn/windows/uwp/design/style/segoe-ui-symbol-font
        /// </summary>
        public string Glyph { get; set; } = string.Empty;

        /// <summary>
        /// 菜单位置
        /// </summary>
        public Position Position { get; set; } = Position.LeftTop;
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

    public enum MenuItemKey
    {
        [Label("主页")]
        Home = 0,

        [Label("版块")]
        Sections,

        [Label("服务")]
        Services,

        [Label("动态")]
        Moments,

        [Label("发帖")]
        Post,

        [Label("消息")]
        Messages,

        [Label("设置")]
        Settings,

        [Label("我的收藏")]
        MyFavorites,

        [Label("我的发表")]
        MyPosts,

        [Label("我的回复")]
        MyReplies,

        [Label("我的插眼")]
        MyMarks,

        [Label("主题筛选")]
        TopicFilter,

        [Label("外观设置")]
        ApperanceSettings,

        [Label("浏览设置")]
        BrowseSettings,

        [Label("通知设置")]
        NotificationSettings,

        [Label("账户设置")]
        AccountSettings,

        [Label("数据与存储")]
        StorageSettings,

        [Label("服务")]
        ServicesSettings,
    }

    public enum Position
    {
        Top = 0,
        LeftTop,
        LeftBottom,
    }
}
