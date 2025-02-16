using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
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

        [RelayCommand]
        private void OpenWebSite(string url) => OperatingSystemHelper.OpenWebsite(url);
    }
}
