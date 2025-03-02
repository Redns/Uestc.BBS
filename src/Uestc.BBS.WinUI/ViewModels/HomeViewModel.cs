using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Services.Forum.TopicList;
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
            ITopicListService topicService,
            AppSettingModel appSettingModel
        )
            : base(logService, topicService, appSettingModel)
        {
            _boardTabItems =
            [
                .. appSettingModel.Appearance.BoardTab.Items.Select(b => new BoardTabItemListView()
                {
                    BoardTabItem = b,
                    IsStaggeredLayoutEnabled = AppSettingModel
                        .Appearance
                        .BoardTab
                        .IsStaggeredLayoutEnabled,
                }),
            ];

            CurrentBoardTabItemModel!.PropertyChanged += (_, _) =>
                OnPropertyChanged(nameof(CurrentBoardTabItemListView));
            AppSettingModel.Appearance.BoardTab.PropertyChanged += (_, e) =>
            {
                if (
                    e.PropertyName
                    == nameof(AppSettingModel.Appearance.BoardTab.IsStaggeredLayoutEnabled)
                )
                {
                    _boardTabItems.ForEach(i =>
                        i.IsStaggeredLayoutEnabled = AppSettingModel
                            .Appearance
                            .BoardTab
                            .IsStaggeredLayoutEnabled
                    );
                    return;
                }
            };
            AppSettingModel.Appearance.BoardTab.Items.CollectionChanged += (_, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        _boardTabItems.AddRange(
                            e.NewItems!.Cast<BoardTabItemModel>()
                                .Select(b => new BoardTabItemListView()
                                {
                                    BoardTabItem = b,
                                    IsStaggeredLayoutEnabled = AppSettingModel
                                        .Appearance
                                        .BoardTab
                                        .IsStaggeredLayoutEnabled,
                                })
                        );
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        _boardTabItems.RemoveAll(b =>
                            e.OldItems!.Cast<BoardTabItemModel>().Contains(b.BoardTabItem)
                        );
                        break;
                }
                OnPropertyChanged(nameof(CurrentBoardTabItemListView));
            };
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
            if (e.ClickedItem as BoardTabItemModel != CurrentBoardTabItemModel)
            {
                return;
            }

            if (CurrentBoardTabItemListView!.Topics!.IsLoading)
            {
                return;
            }

            // FIXME 瀑布流布局下单次点击不会刷新
            for (var i = 0; i < 3; i++)
            {
                await CurrentBoardTabItemListView.Topics.RefreshAsync();
                if (CurrentBoardTabItemListView.Topics.Count > 0)
                {
                    break;
                }
            }
        }

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
