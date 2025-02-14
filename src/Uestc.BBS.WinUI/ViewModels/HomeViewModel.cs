using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.WinUI.Controls;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel : HomeViewModelBase
    {
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private readonly List<BoardTabItemListView> _boardTabItems;

        public BoardTabItemListView? CurrentBoardTabItemListView =>
            _boardTabItems.FirstOrDefault(i => i.BoardTabItem == CurrentBoardTabItemModel);

        public HomeViewModel(
            ILogService logService,
            ITopicService topicService,
            AppSettingModel appSettingModel
        )
            : base(logService, topicService, appSettingModel)
        {
            _boardTabItems =
            [
                .. appSettingModel.Apperance.BoardTabItems.Select(b => new BoardTabItemListView()
                {
                    BoardTabItem = b,
                }),
            ];

            CurrentBoardTabItemModel!.PropertyChanged += (sender, e) =>
                OnPropertyChanged(nameof(CurrentBoardTabItemListView));
        }

        /// <summary>
        /// 切换主题板块
        /// </summary>
        /// <param name="e"></param>
        [RelayCommand]
        private void SwitchBoardTabItem(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is not BoardTabItemModel boardTabItem)
            {
                return;
            }
            CurrentBoardTabItemModel = boardTabItem;
            OnPropertyChanged(nameof(CurrentBoardTabItemListView));
        }

        /// <summary>
        /// 刷新当前板块的帖子列表
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task RefreshBoardTabItems(ItemClickEventArgs e)
        {
            if (
                e.ClickedItem is not BoardTabItemModel boardTabItem
                || boardTabItem != CurrentBoardTabItemModel
            )
            {
                return;
            }

            await CurrentBoardTabItemListView!.Topics!.RefreshAsync();
        }

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
