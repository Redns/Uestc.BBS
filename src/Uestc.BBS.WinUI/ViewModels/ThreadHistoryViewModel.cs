using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Entities;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class ThreadHistoryViewModel(AppSettingModel appSettingModel)
        : ThreadHistoryViewModelBase(appSettingModel)
    {
        [ObservableProperty]
        public partial double BoardTabWidth { get; set; } =
            appSettingModel.Appearance.BoardTab.Width;

        [ObservableProperty]
        public partial ThreadHistoryEntity? CurrentThreadHistory { get; set; }
    }
}
