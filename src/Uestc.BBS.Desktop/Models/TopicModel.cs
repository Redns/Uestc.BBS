using CommunityToolkit.Mvvm.ComponentModel;

namespace Uestc.BBS.Desktop.Models
{
    public partial class TopicModel : ObservableObject
    {
        [ObservableProperty]
        private int _userId;

        [ObservableProperty]
        private int _topicId;

        [ObservableProperty]
        private string _title = string.Empty;
    }
}
