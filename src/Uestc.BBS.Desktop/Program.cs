using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Labs.Controls.Cache;
using Avalonia.Labs.Notifications;
using Avalonia.Media;
using Uestc.BBS.Desktop.Services;

namespace Uestc.BBS.Desktop
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // 应用防多开，重新启动时携带 restart 参数以避免无法启动
            if (
                args.FirstOrDefault() is "restart"
                || Process.GetProcessesByName(AppDomain.CurrentDomain.FriendlyName).Length <= 1
            )
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder
                .Configure<App>()
                .UsePlatformDetect()
                .ConfigureServices()
                .ConfigureFonts(fontManager =>
                {
                    fontManager.AddFontCollection(new HarmonyOSFontCollection());
                })
                .With(
                    new FontManagerOptions()
                    {
                        DefaultFamilyName = "fonts:HarmonyOS Sans#HarmonyOS Sans SC"
                    }
                )
                .WithAppNotifications(
                    new AppNotificationOptions()
                    {
                        Channels = s_channels,
                        AppIcon =
                            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                            + "/avalonia-32.png",
                        AppName = AppDomain.CurrentDomain.FriendlyName
                        ,
                        AppUserModelId = $"com.{AppDomain.CurrentDomain.FriendlyName}"
                    }
                )
                .AfterSetup(builder =>
                {
                    CacheOptions.SetDefault(
                        new CacheOptions()
                        {
                            BaseCachePath = Path.Combine(
                                Path.GetTempPath(),
                                AppDomain.CurrentDomain.FriendlyName
                            )
                        }
                    );
                });

        private static readonly NotificationChannel[] s_channels =
        [
            new NotificationChannel("basic", "Send Notifications", NotificationPriority.High),
            new NotificationChannel(
                "actions",
                "Send Notification with Predefined Actions",
                NotificationPriority.High
            )
            {
                Actions = new List<NativeNotificationAction>
                {
                    new("Hello", "hello"),
                    new("world", "world")
                }
            },
            new NotificationChannel(
                "custom",
                "Send Notification with Custom Actions",
                NotificationPriority.High
            ),
            new NotificationChannel(
                "reply",
                "Send Notification with Reply Action",
                NotificationPriority.High
            )
            {
                Actions = [new NativeNotificationAction("Reply", "reply")]
            },
        ];
    }
}
