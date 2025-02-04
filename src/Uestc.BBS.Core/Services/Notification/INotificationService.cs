namespace Uestc.BBS.Core.Services.Notification
{
    public interface INotificationService
    {
        void Show(string title, string message);

        void Show(string avatar, string title, string message);
    }
}
