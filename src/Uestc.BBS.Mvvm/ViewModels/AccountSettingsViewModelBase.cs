using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public class AccountSettingsViewModelBase(AppSettingModel appSettingModel) : ObservableObject
    {
        public AccountSettingModel AccountSettingModel { get; } = appSettingModel.Account;
    }
}
