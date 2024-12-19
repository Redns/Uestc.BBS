using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.Models;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private SettingsModel _model;

        private readonly AppSetting _appSetting;

        public SettingsViewModel(AppSetting appSetting, SettingsModel model)
        {
            _model = model;
            _appSetting = appSetting;
        }
    }
}
