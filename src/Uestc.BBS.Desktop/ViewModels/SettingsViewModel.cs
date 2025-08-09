using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class SettingsViewModel(Appmanifest appmanifest, AppSettingModel appSettingModel)
        : SettingsViewModelBase(appmanifest, appSettingModel)
    {
        [ObservableProperty]
        public partial bool IsCheckUpgrading { get; set; } = true;
    }
}
