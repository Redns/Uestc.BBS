using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Uestc.BBS.Core.Services;
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
            if (args.FirstOrDefault() is "restart" || Process.GetProcessesByName(AppDomain.CurrentDomain.FriendlyName).Length <= 1)
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .ConfigureServices()
                .ConfigureFonts(fontManager =>
                {
                    fontManager.AddFontCollection(new HarmonyOSFontCollection());
                })
                .With(new FontManagerOptions()
                {
                    DefaultFamilyName = "fonts:HarmonyOS Sans#HarmonyOS Sans SC"
                });
    }
}
