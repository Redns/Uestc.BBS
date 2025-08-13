using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastEnumUtility;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.System;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly AppSetting _appSetting;

        private readonly HttpClient _httpClient;

        private readonly IThreadListService _threadListService;

        private readonly IDailySentenceService _dailySentenceService;

        public string? Avatar => _appSetting.Account.DefaultCredential?.Avatar;

        [ObservableProperty]
        private AppSettingModel _appSettingModel;

        /// <summary>
        /// 侧边栏菜单
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<MenuItemModel>? _menus;

        /// <summary>
        /// 导航页面
        /// </summary>
        [ObservableProperty]
        private UserControl _currentPage;

        public MainWindowViewModel(
            AppSetting appSetting,
            HomeView homeView,
            HttpClient httpClient,
            AppSettingModel appSettingModel,
            IThreadListService threadListService,
            IDailySentenceService dailySentenceService
        )
        {
            _appSetting = appSetting;
            _httpClient = httpClient;
            _currentPage = homeView;
            _appSettingModel = appSettingModel;
            _threadListService = threadListService;
            _dailySentenceService = dailySentenceService;

            Menus =
            [
                .. appSetting
                    .Appearance.MenuItems.Where(m => m.Position == Position.LeftTop)
                    .Concat(
                        appSetting
                            .Appearance.MenuItems.Where(m => m.Position == Position.LeftBottom)
                            .Reverse()
                    )
                    .Select(m => new MenuItemModel
                    {
                        Key = m.Key.ToString(),
                        Name = m.Name,
                        Symbol = m.Symbol,
                        IsActive = false,
                        Dock = m.Position == Position.LeftTop ? Dock.Top : Dock.Bottom,
                        MenuClickCommand = new RelayCommand<MenuItemModel>(menuItem =>
                        {
                            // 已选中菜单
                            if (menuItem is null || menuItem.IsActive)
                            {
                                return;
                            }

                            // 清除先前选中项
                            var beforeActived = Menus?.FirstOrDefault(m => m.IsActive);
                            if (beforeActived is not null)
                            {
                                beforeActived.IsActive = false;
                            }
                            menuItem.IsActive = true;

                            Navigate(menuItem.Key);
                        }),
                    }),
            ];
        }

        /// <summary>
        /// 打开官方论坛链接
        /// </summary>
        [RelayCommand]
        private void OpenOfficialWebsite()
        {
            Process.Start(
                new ProcessStartInfo()
                {
                    FileName = _appSetting.Appearance.OfficialWebsite,
                    UseShellExecute = true,
                }
            );
        }

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="menu"></param>
        [RelayCommand]
        private void Navigate(string menu)
        {
            if (FastEnum.TryParse<MenuItemKey>(menu, out var key) is false)
            {
                return;
            }

            CurrentPage = key switch
            {
                MenuItemKey.Home => ServiceExtension.Services.GetRequiredService<HomeView>(),
                MenuItemKey.Sections =>
                    ServiceExtension.Services.GetRequiredService<SectionsView>(),
                MenuItemKey.Services =>
                    ServiceExtension.Services.GetRequiredService<ServicesView>(),
                MenuItemKey.Moments => ServiceExtension.Services.GetRequiredService<MomentsView>(),
                MenuItemKey.Post => ServiceExtension.Services.GetRequiredService<PostView>(),
                MenuItemKey.Messages =>
                    ServiceExtension.Services.GetRequiredService<MessagesView>(),
                MenuItemKey.Settings =>
                    ServiceExtension.Services.GetRequiredService<SettingsView>(),
                _ => CurrentPage,
            };
        }

        /// <summary>
        /// 主题切换
        /// </summary>
        [RelayCommand]
        private void SwitchTheme()
        {
            Application.Current!.RequestedThemeVariant =
                Application.Current.ActualThemeVariant == ThemeVariant.Light
                    ? ThemeVariant.Dark
                    : ThemeVariant.Light;

            //AppSettingModel.ThemeColor =
            //    Application.Current.RequestedThemeVariant == ThemeVariant.Light
            //        ? ThemeColor.Light
            //        : ThemeColor.Dark;
        }

        /// <summary>
        /// 窗口固定切换
        /// </summary>
        [RelayCommand]
        private void SwitchPinWindow() { }
        //AppSettingModel.IsWindowPinned = !AppSettingModel.IsWindowPinned;
    }

    public partial class MenuItemModel : ObservableObject
    {
        public string Key { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Symbol { get; set; } = string.Empty;

        public Dock Dock { get; set; } = Dock.Top;

        [ObservableProperty]
        private bool _isActive = false;

        public IRelayCommand? MenuClickCommand { get; set; }

        public string CommandParameter { get; set; } = string.Empty;
    }
}
