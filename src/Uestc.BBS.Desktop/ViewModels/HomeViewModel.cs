using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
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
        private Task<List<BoardTabItemModel>> _boardTabItems;

        public HomeViewModel(ITopicService topicService)
        {
            _topicService = topicService;
            _boardTabItems = Task.Run(async () =>
            {
                var tabItems = new List<BoardTabItemModel>
                {
                    new()
                    {
                        Key = "最新发表",
                        Board = Board.Latest,
                        SortType = TopicSortType.New,
                        PageSize = 15
                    },
                    new()
                    {
                        Key = "最新回复",
                        Board = Board.Latest,
                        SortType = TopicSortType.All,
                        PageSize = 15
                    },
                    new()
                    {
                        Key = "热门",
                        Board = Board.Anonymous,
                        SortType = TopicSortType.All,
                        PageSize = 15
                    },
                    new()
                    {
                        Key = "精华",
                        Board = Board.Transportation,
                        SortType = TopicSortType.Essence,
                        PageSize = 15
                    },
                    new()
                    {
                        Key = "淘专辑",
                        Board = Board.ExamiHome,
                        SortType = TopicSortType.New,
                        PageSize = 15
                    }
                };
                var tabItemLoadTopicsTasks = tabItems.Select(tabItem =>
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
                await Task.WhenAll(tabItemLoadTopicsTasks);

                return tabItems;
            });
        }

        [RelayCommand]
        private async Task LoadTopicsAsync(ScrollChangedEventArgs args)
        {

        }
    }
}
