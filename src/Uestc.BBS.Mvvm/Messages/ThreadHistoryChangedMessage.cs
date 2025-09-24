using CommunityToolkit.Mvvm.Messaging.Messages;
using Uestc.BBS.Sdk.Services.Thread;

namespace Uestc.BBS.Mvvm.Messages
{
    public class ThreadHistoryChangedMessage(ThreadOverview thread)
        : ValueChangedMessage<ThreadOverview>(thread) { }
}
