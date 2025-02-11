using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Core.Services.Notification;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class MainViewModel : MainViewModelBase
    {
        /// <summary>
        /// 顶部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> TopMenuItems =>
            [.. AppSettingModel.Apperance.MenuItems.Where(m => m.DockTop)];

        /// <summary>
        /// 底部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> FooterMenuItems =>
            [.. AppSettingModel.Apperance.MenuItems.Where(m => !m.DockTop)];

        /// <summary>
        /// 调度任务队列
        /// </summary>
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public MainViewModel(
            AppSettingModel appSettingModel,
            ILogService logService,
            INavigateService navigateService,
            INotificationService notificationService,
            IDailySentenceService dailySentenceService
        )
            : base(
                appSettingModel,
                logService,
                navigateService,
                notificationService,
                dailySentenceService
            )
        {
            appSettingModel.Apperance.MenuItems.CollectionChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(TopMenuItems));
                OnPropertyChanged(nameof(FooterMenuItems));
            };
        }

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
