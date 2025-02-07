using CommunityToolkit.Mvvm.ComponentModel;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract class SettingsViewModel : ObservableObject
    {
        public abstract Task CheckUpdateAsync();
    }
}
