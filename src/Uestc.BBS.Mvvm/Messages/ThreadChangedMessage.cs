using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Uestc.BBS.Mvvm.Messages
{
    public class ThreadChangedMessage(uint id) : ValueChangedMessage<uint>(id) { }
}
