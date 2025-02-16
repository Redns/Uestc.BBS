using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public partial class ApperanceSettingsViewModelBase(AppSettingModel appSettingModel)
        : ObservableObject
    {
        public ApperanceSettingModel ApperanceSettingModel { get; init; } =
            appSettingModel.Apperance;
    }
}
