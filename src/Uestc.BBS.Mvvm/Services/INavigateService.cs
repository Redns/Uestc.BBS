using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Mvvm.Services
{
    public interface INavigateService<TContent>
        where TContent : class
    {
        TContent Navigate(MenuItemKey key);
    }
}
