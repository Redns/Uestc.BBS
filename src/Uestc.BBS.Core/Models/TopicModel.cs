using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Core.Models
{
    public partial class TopicModel : ObservableObject
    {
        [ObservableProperty]
        private int _userId;

        [ObservableProperty]
        private string _userName = string.Empty;

        [ObservableProperty]
        private string _userAvatar = string.Empty;

        [ObservableProperty]
        private int _topicId;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BoardName))]
        private Board _board = Board.WaterHome;

        public string BoardName => Board.GetName();

        [ObservableProperty]
        private string _type = string.Empty;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private DateTime _lastReplyDate;


    }
}
