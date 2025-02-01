using CommunityToolkit.Mvvm.ComponentModel;

namespace Uestc.BBS.Core.Services.NavigateService
{
    public interface INavigateService
    {
        ObservableObject Navigate(string page);
    }
}
