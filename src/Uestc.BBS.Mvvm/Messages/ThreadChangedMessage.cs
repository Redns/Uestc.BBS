using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Uestc.BBS.Mvvm.Messages
{
    public class ThreadChangedMessage(uint threadId) : ValueChangedMessage<uint>(threadId) { }
}
