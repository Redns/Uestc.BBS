using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.Forum;

namespace Uestc.BBS.Mvvm.Models
{
    /// <summary>
    /// 外观设置
    /// </summary>
    public class AppearanceSettingModel : ObservableObject
    {
        private readonly AppearanceSetting _apperanceSetting;

        public AppearanceSettingModel(AppearanceSetting apperanceSetting)
        {
            _apperanceSetting = apperanceSetting;

            SearchBar = new SearchBarSettingModel(apperanceSetting.SearchBar);

            MenuItems = [.. apperanceSetting.MenuItems.Select(item => new MenuItemModel(item))];
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
            BoardTab = new BoardTabSettingModel(apperanceSetting.BoardTab);
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
                    (s, e) => s.ThemeColor = e
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
                    (s, e) => s.IsTopNavigateBarEnabled = e
                );
        }

        /// <summary>
        /// 搜索栏
        /// </summary>
        public SearchBarSettingModel SearchBar { get; init; }

        /// <summary>
        /// 侧边栏菜单列表
        /// </summary>
        public ObservableCollection<MenuItemModel> MenuItems { get; init; }

        /// <summary>
        /// 首页版块 Tab 栏设置
        /// </summary>
        public BoardTabSettingModel BoardTab { get; init; }

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
                    (s, e) => s.OfficialWebsite = e
                );
        }
    }

    public class BoardTabSettingModel : ObservableObject
    {
        private readonly BoardTabSetting _setting;

        public BoardTabSettingModel(BoardTabSetting setting)
        {
            _setting = setting;

            Items = [.. _setting.Items.Select(item => new BoardTabItemModel(item))];
            Items.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        _setting.Items.AddRange(
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
                            _setting.Items.Remove(item);
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
        /// 首页版块 Tab 栏宽度
        /// </summary>
        public double Width
        {
            get => _setting.Width;
            set => SetProperty(_setting.Width, value, _setting, (s, v) => s.Width = v);
        }

        /// <summary>
        /// 首页版块 Tab 栏是否启用瀑布流
        /// </summary>
        public bool IsStaggeredLayoutEnabled
        {
            get => _setting.IsStaggeredLayoutEnabled;
            set =>
                SetProperty(
                    _setting.IsStaggeredLayoutEnabled,
                    value,
                    _setting,
                    (s, v) => s.IsStaggeredLayoutEnabled = v
                );
        }

        /// <summary>
        /// 首页版块 Tab 栏
        /// </summary>
        public ObservableCollection<BoardTabItemModel> Items { get; init; } = [];
    }

    public class MenuItemModel(MenuItem menuItem) : ObservableObject
    {
        public MenuItem MenuItem => menuItem;

        public MenuItemKey Key
        {
            get => menuItem.Key;
            set => SetProperty(menuItem.Key, value, menuItem, (s, e) => s.Key = e);
        }

        public string Name
        {
            get => menuItem.Name;
            set => SetProperty(menuItem.Name, value, menuItem, (s, e) => s.Name = e);
        }

        public string Symbol
        {
            get => menuItem.Symbol;
            set => SetProperty(menuItem.Symbol, value, menuItem, (s, e) => s.Symbol = e);
        }

        public string Glyph
        {
            get => menuItem.Glyph;
            set => SetProperty(menuItem.Glyph, value, menuItem, (s, e) => s.Glyph = e);
        }

        public Position Position
        {
            get => menuItem.Position;
            set => SetProperty(menuItem.Position, value, menuItem, (s, e) => s.Position = e);
        }
    }

    public class BoardTabItemModel(BoardTabItem boardTabItem) : ObservableObject
    {
        public BoardTabItem BoardTabItem => boardTabItem;

        public string Name
        {
            get => boardTabItem.Name;
            set => SetProperty(boardTabItem.Name, value, boardTabItem, (s, e) => s.Name = e);
        }

        public string Route
        {
            get => boardTabItem.Route;
            set => SetProperty(boardTabItem.Route, value, boardTabItem, (s, e) => s.Route = e);
        }

        public Board Board
        {
            get => boardTabItem.Board;
            set => SetProperty(boardTabItem.Board, value, boardTabItem, (s, e) => s.Board = e);
        }

        public TopicSortType SortType
        {
            get => boardTabItem.SortType;
            set =>
                SetProperty(boardTabItem.SortType, value, boardTabItem, (s, e) => s.SortType = e);
        }

        public uint PageSize
        {
            get => boardTabItem.PageSize;
            set =>
                SetProperty(boardTabItem.PageSize, value, boardTabItem, (s, e) => s.PageSize = e);
        }

        public bool RequirePreviewSources
        {
            get => boardTabItem.RequirePreviewSources;
            set =>
                SetProperty(
                    boardTabItem.RequirePreviewSources,
                    value,
                    boardTabItem,
                    (s, e) => s.RequirePreviewSources = e
                );
        }

        public uint ModuleId
        {
            get => boardTabItem.ModuleId;
            set =>
                SetProperty(boardTabItem.ModuleId, value, boardTabItem, (s, e) => s.ModuleId = e);
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        public ObservableCollection<TopicOverview> Topics { get; set; } = [];
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
                    (s, e) => s.IsDailySentenceEnabled = e
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
                    (s, e) => s.DailySentenceUpdateTimeInterval = e
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
                    (s, e) => s.IsSearchHistoryEnabled = e
                );
        }
    }
}
