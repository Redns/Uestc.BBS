using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public class ServicesViewModelBase(AppSettingModel appSettingModel) : ObservableObject
    {
        public ServicesSettingModel ServicesSettingModel { get; init; } = appSettingModel.Services;
    }
}
