using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class ServicesSettingsViewModel(
        ILogService logService,
        AppSettingModel appSettingModel
    ) : ServicesSettingsViewModelBase(logService, appSettingModel) { }
}
