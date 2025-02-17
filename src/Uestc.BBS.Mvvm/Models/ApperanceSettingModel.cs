using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Mvvm.Models
{
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
                    (setting, website) => setting.OfficialWebsite = website
                );
        }
    }

    public class BoardTabSettingModel : ObservableObject
    {
        private readonly BoardTabSetting _setting;

        public BoardTabSettingModel(BoardTabSetting setting)
        {
            _setting = setting;

            Items = [.. _setting.Items.Select(item => new BoardTabItemModel(item)),];
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

        public uint PageSize
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

        public uint ModuleId
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
}
