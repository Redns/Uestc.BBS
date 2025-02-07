using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class TopicModel : ObservableObject
    {
        [ObservableProperty]
        public partial int UserId { get; set; }

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
