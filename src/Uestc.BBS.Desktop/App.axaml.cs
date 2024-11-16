using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Desktop.Services;
using Uestc.BBS.Desktop.Services.StartupService;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 初始化服务
        ServiceExtension.ConfigureServices(s => ConfigureServices(s));

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        DataContext = ServiceExtension.GetService<AppViewModel>();

        // TODO 检查用户是否授权
        var isUserAuthed = false;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            desktop.MainWindow = isUserAuthed ? ServiceExtension.GetRequiredService<MainWindow>() : ServiceExtension.GetRequiredService<AuthWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// 注入公共依赖
    /// </summary>
    /// <returns></returns>
    private static void ConfigureServices(ServiceCollection collection)
    {
        // View & ViewModel
        collection.AddSingleton<AppViewModel>();
        collection.AddSingleton<AuthWindow>();
        collection.AddSingleton<AuthViewModel>();
        collection.AddSingleton<MainWindow>();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<HomeView>();
        // 自启动
        collection.AddSingleton<IStartupService>(startup =>
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
        collection.AddSingleton<ILogService>(logger => new NLogService(LogManager.GetLogger("*")));
    }
}
