using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class SettingsViewModelBase(AppSettingModel appSettingModel)
        : ObservableObject
    {
        public AppSettingModel AppSettingModel { get; init; } = appSettingModel;

        public abstract Task CheckUpdateAsync();
    }
}
