using Uestc.BBS.Core;

namespace Uestc.BBS.Mvvm.Services
{
    public interface INavigateService<TContent>
        where TContent : class
    {
        TContent Navigate(MenuItemKey key);
    }
}
