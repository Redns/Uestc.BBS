namespace Uestc.BBS.Core.Services.System
{
    public interface INotificationService
    {
        void Show(string title, string message);

        void Show(string avatar, string title, string message);
    }
}
