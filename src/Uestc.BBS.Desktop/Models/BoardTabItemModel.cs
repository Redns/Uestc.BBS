using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Desktop.Models
{
    public partial class BoardTabItemModel : ObservableObject
    {
        [ObservableProperty]
        private string _key = string.Empty;

        [ObservableProperty]
        private Board _board = Board.Latest;

        [ObservableProperty]
        private TopicSortType _sortType = TopicSortType.New;

        [ObservableProperty]
        private uint _pageSize = 15;

        [ObservableProperty]
        private Task<ObservableCollection<TopicOverview>> _topics = Task.FromResult(
            new ObservableCollection<TopicOverview>()
        );
    }
}
