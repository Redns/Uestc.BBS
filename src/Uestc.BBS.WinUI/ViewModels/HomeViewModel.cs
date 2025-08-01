using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Core.Services.Forum.TopicList;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.WinUI.Controls;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel : HomeViewModelBase
    {

        public BoardTabItemListView CurrentBoardTabItemListView =>
            _boardTabItemListViewList.FirstOrDefault(b =>
                b.BoardTabItem == CurrentBoardTabItemModel
            ) ?? _boardTabItemListViewList[0];

        private List<BoardTabItemListView> _boardTabItemListViewList;

        public HomeViewModel(
            ILogService logService,
            ITopicService topicService,
            ITopicListService topicListService,
            AppSettingModel appSettingModel
        )
            : base(logService, topicService, topicListService, appSettingModel)
        {
            _boardTabItemListViewList =
            [
                .. appSettingModel.Appearance.BoardTab.Items.Select(b => new BoardTabItemListView()
                {
                    BoardTabItem = b,
                }),
            ];
        }

        /// <summary>
        /// 切换主题板块
        /// </summary>
        /// <param name="e"></param>
        [RelayCommand]
        private void SwitchBoardTabItem(SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is not BoardTabItemModel boardTabItem)
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
            if (e.ClickedItem as BoardTabItemModel != CurrentBoardTabItemModel)
            {
                return;
            }

            if (CurrentBoardTabItemListView!.Topics!.IsLoading)
            {
                return;
            }

            await CurrentBoardTabItemListView.Topics.RefreshAsync();
        }
    }
}
