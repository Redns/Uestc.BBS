using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class BoardTabItemModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Route { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Board Board { get; set; } = Board.Latest;

        [ObservableProperty]
        public partial TopicSortType SortType { get; set; } = TopicSortType.New;

        [ObservableProperty]
        public partial uint PageSize { get; set; } = 15;

        [ObservableProperty]
        public partial bool RequirePreviewSources { get; set; } = false;

        [ObservableProperty]
        public partial uint ModuleId { get; set; } = 0;

        /// <summary>
        /// 是否正在加载数据
        /// </summary>
        [ObservableProperty]
        public partial bool IsLoading { get; set; } = false;

        [ObservableProperty]
        public partial ObservableCollection<TopicOverview> Topics { get; set; } = [];
    }
}
