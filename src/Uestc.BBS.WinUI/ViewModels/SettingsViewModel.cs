using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class SettingsViewModel(
        AppSetting appSetting,
        Appmanifest appmanifest,
        AppSettingModel appSettingModel,
        ILogService logService
    ) : SettingsViewModelBase(appSetting, appmanifest, appSettingModel, logService) { }
}
