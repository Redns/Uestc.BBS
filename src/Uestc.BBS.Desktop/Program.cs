using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Uestc.BBS.Desktop.Services;
using Uestc.BBS.Desktop.Services.StartupService;
using Uestc.BBS.Services;

namespace Uestc.BBS.Desktop;

class Program
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
    public static AppBuilder BuildAvaloniaApp()
    {
        ConfigureServices();
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .ConfigureFonts(fontManager =>
            {
                fontManager.AddFontCollection(new HarmonyOSFontCollection());
            })
            .With(new FontManagerOptions()
            {
                DefaultFamilyName = "fonts:HarmonyOS Sans#HarmonyOS Sans SC"
            });
    }

    private static void ConfigureServices()
    {
        // 日志
        App.ServiceCollection.AddSingleton<ILogService>(logger => new NLogService(LogManager.GetLogger("*")));
        // 自启动
        App.ServiceCollection.AddSingleton<IStartupService>(startup =>
        {
            var startupInfo = new StartupInfo
            {
                Name = AppDomain.CurrentDomain.FriendlyName,
                Description = $"{AppDomain.CurrentDomain.FriendlyName} startup service",
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                ApplicationPath = Environment.ProcessPath!
            };

            if (OperatingSystem.IsWindows())
            {
                return new WindowsStartupService(startupInfo);
            }
            else if (OperatingSystem.IsLinux())
            {
                return new LinuxStartupService(startupInfo);
            }
            else if (OperatingSystem.IsMacCatalyst())
            {
                return new MacCatalystStartupService();
            }
            throw new PlatformNotSupportedException(Environment.OSVersion.VersionString);
        });
    }
}
