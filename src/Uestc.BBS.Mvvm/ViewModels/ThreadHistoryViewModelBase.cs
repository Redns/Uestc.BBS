using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public class ThreadHistoryViewModelBase(AppSettingModel appSettingModel) : ObservableObject
    {
        public AccountSettingModel AppSettingModel { get; } = appSettingModel.Account;
    }
}
