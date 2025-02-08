using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class SettingsViewModelBase(
        Appmanifest appmanifest,
        AppSettingModel appSettingModel
    ) : ObservableObject
    {
        public Appmanifest Appmanifest { get; init; } = appmanifest;

        public AppSettingModel AppSettingModel { get; init; } = appSettingModel;

        public abstract Task CheckUpdateAsync();
    }
}
