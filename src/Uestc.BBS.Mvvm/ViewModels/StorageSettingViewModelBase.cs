using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public class StorageSettingViewModelBase(AppSettingModel appSettingModel) : ObservableObject
    {
        public StorageSettingModel StorageSettingModel { get; } = appSettingModel.Storage;
    }
}
