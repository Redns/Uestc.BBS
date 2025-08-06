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
    public partial class HomeViewModel(
        ILogService logService,
        ITopicService topicService,
        ITopicListService topicListService,
        AppSettingModel appSettingModel
    )
        : HomeViewModelBase<BoardTabItemListView>(
            logService,
            topicService,
            topicListService,
            model => new BoardTabItemListView() { BoardTabItem = model },
            view => view.BoardTabItem,
            appSettingModel
        )
    {
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
