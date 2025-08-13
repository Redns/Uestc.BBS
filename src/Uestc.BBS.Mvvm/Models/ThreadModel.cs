using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Sdk.Services.Thread;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class ThreadModel(ThreadOverview thread) : ObservableObject
    {
        [ObservableProperty]
        public partial uint UserId { get; set; } = thread.Uid;

        [ObservableProperty]
        public partial string UserName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string UserAvatar { get; set; } = string.Empty;

        [ObservableProperty]
        public partial int TopicId { get; set; }

        [ObservableProperty]
        public partial Board Board { get; set; } = Board.WaterHome;

        [ObservableProperty]
        public partial string BoardName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTime LastReplyDate { get; set; } = DateTime.Now;
    }
}
