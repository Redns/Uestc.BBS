using CommunityToolkit.Mvvm.ComponentModel;

namespace Uestc.BBS.Core.ViewModels
{
    public abstract class SettingsViewModel : ObservableObject
    {
        public abstract Task CheckUpdateAsync();
    }
}
