using CommunityToolkit.Mvvm.ComponentModel;

namespace Uestc.BBS.Desktop.Models
{
    public partial class TopicModel : ObservableObject
    {
        [ObservableProperty]
        public partial int UserId { get; set; }

        [ObservableProperty]
        public partial int TopicId { get; set; }

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;
    }
}
