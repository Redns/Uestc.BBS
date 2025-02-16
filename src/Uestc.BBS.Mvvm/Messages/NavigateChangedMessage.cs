using CommunityToolkit.Mvvm.Messaging.Messages;
using Uestc.BBS.Core;

namespace Uestc.BBS.Mvvm.Messages
{
    public class NavigateChangedMessage(MenuItemKey menu)
        : ValueChangedMessage<MenuItemKey>(menu) { }
}
