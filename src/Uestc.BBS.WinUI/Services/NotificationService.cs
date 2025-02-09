using System;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Uestc.BBS.Core.Services.Notification;

namespace Uestc.BBS.WinUI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppNotificationManager _appNotificationManager =
            AppNotificationManager.Default;

        public NotificationService(string appName, Uri iconUri)
        {
            _appNotificationManager.Register(appName, iconUri);
            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
                _appNotificationManager.Unregister();
        }

        ~NotificationService()
        {
            _appNotificationManager.Unregister();
        }

        public void Show(string title, string message) =>
            AppNotificationManager.Default.Show(
                new AppNotificationBuilder().AddText(title).AddText(message).BuildNotification()
            );

        public void Show(string avatar, string title, string message) =>
            AppNotificationManager.Default.Show(
                new AppNotificationBuilder()
                    .SetAppLogoOverride(new Uri(avatar), AppNotificationImageCrop.Circle)
                    .AddText(title)
                    .AddText(message)
                    .BuildNotification()
            );
    }
}
