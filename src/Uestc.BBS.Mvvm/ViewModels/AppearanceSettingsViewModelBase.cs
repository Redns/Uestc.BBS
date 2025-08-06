using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public partial class AppearanceSettingsViewModelBase(AppSettingModel appSettingModel)
        : ObservableObject
    {
        public AppearanceSettingModel ApperanceSettingModel { get; } = appSettingModel.Appearance;
    }
}
