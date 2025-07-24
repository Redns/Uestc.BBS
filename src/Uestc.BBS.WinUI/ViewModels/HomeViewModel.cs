using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.Core.Services.Forum.TopicList;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel : HomeViewModelBase
    {
        public HomeViewModel(
            ILogService logService,
            ITopicService topicService,
            ITopicListService topicListService,
            AppSettingModel appSettingModel
        )
            : base(logService, topicService, topicListService, appSettingModel) { }

        /// <summary>
        /// 切换主题板块
        /// </summary>
        /// <param name="e"></param>
        [RelayCommand]
        private void SwitchBoardTabItem(SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.FirstOrDefault() is not BoardTabItemModel boardTabItem)
            //{
            //    return;
            //}
            //CurrentBoardTabItemModel = boardTabItem;
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

            //if (CurrentBoardTabItemListView!.Topics!.IsLoading)
            //{
            //    return;
            //}

            //// FIXME 瀑布流布局下单次点击不会刷新
            //for (var i = 0; i < 3; i++)
            //{
            //    await CurrentBoardTabItemListView.Topics.RefreshAsync();
            //    if (CurrentBoardTabItemListView.Topics.Count > 0)
            //    {
            //        break;
            //    }
            //}
        }
    }
}
