using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ITopicService _topicService;

        [ObservableProperty]
        private Task<ObservableCollection<TopicOverview>> _topics;

        public HomeViewModel(ITopicService topicService)
        {
            _topicService = topicService;
            _topics = _topicService
                .GetTopicsAsync(pageSize: 30, boardId: Board.Latest, sortby: TopicSortType.All)
                .ContinueWith(t => new ObservableCollection<TopicOverview>(t.Result?.List ?? []));
        }
    }
}
