using Avalonia;
using DeviceId;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Services;
using Uestc.BBS.Desktop.Services.StartupService;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Desktop
{
    public static class AppExtension
    {
        /// <summary>
        /// 初始化桌面端服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AppBuilder ConfigureServices(this AppBuilder builder)
        {
            ServiceExtension.ConfigureCommonServices();

            // View & ViewModel
            ServiceExtension.ServiceCollection.AddSingleton<AppViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<AuthWindow>();
            ServiceExtension.ServiceCollection.AddSingleton<AuthViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<MainWindow>();
            ServiceExtension.ServiceCollection.AddSingleton<MainWindowViewModel>();
            ServiceExtension.ServiceCollection.AddSingleton<HomeView>();
            // DeviceId
            ServiceExtension.ServiceCollection.AddSingleton(deviceId => new DeviceIdBuilder().AddMachineName().AddOsVersion()
                .OnWindows(windows => windows
                    .AddProcessorId()
                    .AddMotherboardSerialNumber()
                    .AddSystemDriveSerialNumber())
                .OnLinux(linux => linux
                    .AddMotherboardSerialNumber())
                .OnMac(mac => mac
                    .AddSystemDriveSerialNumber()
                    .AddPlatformSerialNumber())
                .ToString());
            // 自启动
            ServiceExtension.ServiceCollection.AddSingleton<IStartupService>(startup =>
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
                else
                {
                    return new MacCatalystStartupService();
                }
            });
            // 日志
            ServiceExtension.ServiceCollection.AddSingleton<ILogService>(logger => new NLogService(LogManager.GetLogger("*")));

            return builder;
        }
    }
}
