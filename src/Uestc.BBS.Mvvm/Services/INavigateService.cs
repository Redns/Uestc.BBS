using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;

namespace Uestc.BBS.Mvvm.Services
{
    public interface INavigateService
    {
        ObservableObject Navigate(MenuItemKey key);
    }
}
