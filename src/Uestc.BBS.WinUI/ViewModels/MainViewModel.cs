using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.Services;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.System;
using Uestc.BBS.Sdk.Services.User.Friend;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class MainViewModel : MainViewModelBase<Page>
    {
        /// <summary>
        /// 顶部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> TopMenuItems =>
            [.. AppSettingModel.Appearance.MenuItems.Where(m => m.Position is Position.Top)];

        /// <summary>
        /// 侧边顶部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> LeftTopMenuItems =>
            [.. AppSettingModel.Appearance.MenuItems.Where(m => m.Position is Position.LeftTop)];

        /// <summary>
        /// 侧边底部菜单项
        /// </summary>
        public ObservableCollection<MenuItemModel> LeftFooterMenuItems =>
            [.. AppSettingModel.Appearance.MenuItems.Where(m => m.Position is Position.LeftBottom)];

        /// <summary>
        /// 调度任务队列
        /// </summary>
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public MainViewModel(
            Uri baseUri,
            AppSettingModel appSettingModel,
            ILogService logService,
            IFriendListService friendListService,
            [FromKeyedServices(ServiceExtensions.WEB_API)] IGlobalStatusService globalStatusService,
            INotificationService notificationService,
            IDailySentenceService dailySentenceService,
            INavigateService<Page> navigateService
        )
            : base(
                baseUri,
                appSettingModel,
                logService,
                friendListService,
                globalStatusService,
                notificationService,
                dailySentenceService,
                navigateService
            )
        {
            // FIXME 修改菜单项后 CurrentMenuItem 在 set 时 value 为 null
            appSettingModel.Appearance.MenuItems.CollectionChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(TopMenuItems));
                OnPropertyChanged(nameof(LeftTopMenuItems));
                OnPropertyChanged(nameof(LeftFooterMenuItems));
            };
        }

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
