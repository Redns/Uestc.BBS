using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;
using Uestc.BBS.WinUI.Controls;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel(
        ILogService logService,
        IThreadListService threadListService,
        AppSettingModel appSettingModel
    )
        : HomeViewModelBase<BoardTabItemListView>(
            logService,
            threadListService,
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
