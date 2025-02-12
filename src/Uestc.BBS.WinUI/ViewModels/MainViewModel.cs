using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class MainViewModel : MainViewModelBase<Page>
    {
        /// <summary>
        /// 顶部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> TopMenuItems =>
            [.. AppSettingModel.Apperance.MenuItems.Where(m => m.Position is Position.Top)];

        /// <summary>
        /// 侧边顶部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> LeftTopMenuItems =>
            [.. AppSettingModel.Apperance.MenuItems.Where(m => m.Position is Position.LeftTop)];

        /// <summary>
        /// 侧边底部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> LeftFooterMenuItems =>
            [.. AppSettingModel.Apperance.MenuItems.Where(m => m.Position is Position.LeftBottom)];

        /// <summary>
        /// 调度任务队列
        /// </summary>
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public MainViewModel(
            AppSettingModel appSettingModel,
            ILogService logService,
            INotificationService notificationService,
            IDailySentenceService dailySentenceService,
            INavigateService<Page> navigateService
        )
            : base(
                appSettingModel,
                logService,
                notificationService,
                dailySentenceService,
                navigateService
            )
        {
            appSettingModel.Apperance.MenuItems.CollectionChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(TopMenuItems));
                OnPropertyChanged(nameof(LeftTopMenuItems));
                OnPropertyChanged(nameof(LeftFooterMenuItems));
            };
        }

        [RelayCommand]
        private void SwitchTopMenu(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is not MenuItemModel menuItem)
            {
                return;
            }
            CurrentMenuItem = menuItem;
        }

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
