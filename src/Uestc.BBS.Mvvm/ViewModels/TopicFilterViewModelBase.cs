using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public class TopicFilterViewModelBase(AppSettingModel appSettingModel) : ObservableObject
    {
        public AccountSettingModel AppSettingModel { get; init; } = appSettingModel.Account;
    }
}
