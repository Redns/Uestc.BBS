using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FastEnumUtility;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class SettingsViewModel(Appmanifest appmanifest, AppSettingModel appSettingModel)
        : SettingsViewModelBase(appmanifest, appSettingModel)
    {
        [RelayCommand]
        private void NavigateToSubSettingsOverlay(string key) =>
            StrongReferenceMessenger.Default.Send(
                new NavigateChangedMessage(FastEnum.Parse<MenuItemKey>(key))
            );
    }
}
