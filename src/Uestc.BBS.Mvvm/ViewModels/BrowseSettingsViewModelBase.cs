using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public class BrowseSettingsViewModelBase(AppSettingModel appSettingModel) : ObservableObject
    {
        public BrowseSettingModel BrowsingSettingModel { get; init; } = appSettingModel.Browse;

        public AppearanceSettingModel AppearanceSettingModel { get; init; } =
            appSettingModel.Appearance;
    }
}
