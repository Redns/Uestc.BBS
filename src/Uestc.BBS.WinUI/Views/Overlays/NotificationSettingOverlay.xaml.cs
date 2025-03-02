using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class NotificationSettingOverlay : Page
    {
        private NotificationSettingModel Model { get; init; }

        public NotificationSettingOverlay(AppSettingModel model)
        {
            InitializeComponent();

            Model = model.Notification;
        }
    }
}
