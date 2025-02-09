using CommunityToolkit.Mvvm.ComponentModel;

namespace Uestc.BBS.Mvvm.Services
{
    public interface INavigateService
    {
        ObservableObject Navigate(string view);
    }
}
