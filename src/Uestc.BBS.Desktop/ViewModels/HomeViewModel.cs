using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Labs.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Desktop.Models;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ITopicService _topicService;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private BoardTabItemModel? _currentBoardTabItemModel;

        [ObservableProperty]
        private ObservableCollection<BoardTabItemModel> _boardTabItems;

        public HomeViewModel(ITopicService topicService)
        {
            _topicService = topicService;
            _boardTabItems =
            [
                new()
                {
                    Key = "最新发表",
                    Board = Board.Latest,
                    SortType = TopicSortType.New,
                    PageSize = 15,
                },
                new()
                {
                    Key = "最新回复",
                    Board = Board.Latest,
                    SortType = TopicSortType.All,
                    PageSize = 15,
                },
                new()
                {
                    Key = "热门",
                    Board = Board.Anonymous,
                    SortType = TopicSortType.All,
                    PageSize = 15,
                },
                new()
                {
                    Key = "精华",
                    Board = Board.Transportation,
                    SortType = TopicSortType.Essence,
                    PageSize = 15,
                },
                new()
                {
                    Key = "淘专辑",
                    Board = Board.ExamiHome,
                    SortType = TopicSortType.New,
                    PageSize = 15,
                },
            ];

            // 加载板块帖子
            Task.Run(async () =>
            {
                IsLoading = true;

                var tabItemLoadTopicsTasks = _boardTabItems.Select(tabItem =>
                    tabItem.Topics = _topicService
                        .GetTopicsAsync(
                            pageSize: tabItem.PageSize,
                            boardId: tabItem.Board,
                            sortby: tabItem.SortType
                        )
                        .ContinueWith(t => new ObservableCollection<TopicOverview>(
                            t.Result?.List ?? []
                        ))
                );
                await Task.WhenAll(tabItemLoadTopicsTasks).ContinueWith(t => IsLoading = false);
            });
        }

        [RelayCommand]
        private async Task LoadTopicsAsync(ScrollChangedEventArgs args)
        {
            if (args.Source is ScrollViewer scrollViewer)
            {
                if (
                    scrollViewer.Offset.Length
                    < scrollViewer.Extent.Height - (scrollViewer.DesiredSize.Height * 2)
                )
                {
                    return;
                }

                IsLoading = true;
                var currentTopics = await CurrentBoardTabItemModel.Topics;

                foreach (
                    var topic in await _topicService
                        .GetTopicsAsync(
                            page: (uint)CurrentBoardTabItemModel.Topics.Result.Count
                                / CurrentBoardTabItemModel.PageSize
                                + 1,
                            pageSize: CurrentBoardTabItemModel.PageSize,
                            boardId: CurrentBoardTabItemModel.Board,
                            sortby: CurrentBoardTabItemModel.SortType
                        )
                        .ContinueWith(t => new ObservableCollection<TopicOverview>(
                            t.Result?.List ?? []
                        ))
                )
                {
                    CurrentBoardTabItemModel.Topics.Result.Add(topic);
                }
                IsLoading = false;
            }
        }
    }
}
