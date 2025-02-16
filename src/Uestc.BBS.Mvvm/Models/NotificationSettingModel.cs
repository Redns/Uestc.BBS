using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;

namespace Uestc.BBS.Mvvm.Models
{
    public class NotificationSettingModel(NotificationSetting setting) : ObservableObject
    {
        private readonly NotificationSetting _setting = setting;
    }
}
