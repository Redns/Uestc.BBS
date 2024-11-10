using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Net.Http;
using Uestc.BBS.Core.Services;
using Uestc.BBS.Desktop.Services;
using Uestc.BBS.Desktop.Services.StartupService;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Desktop;

public partial class App : Application
{
    public static ServiceCollection ServiceCollection { get; } = new();
    public static ServiceProvider Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 初始化服务
        Services = ConfigureServices();

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        DataContext = Services.GetService<AppViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            desktop.MainWindow = Services.GetService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// 注入公共依赖
    /// </summary>
    /// <returns></returns>
    private static ServiceProvider ConfigureServices()
    {
        // 日志
        ServiceCollection.AddSingleton<ILogService>(logger => new NLogService(LogManager.GetLogger("*")));
        // 自启动
        ServiceCollection.AddSingleton<IStartupService>(startup =>
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
        // View & ViewModel
        ServiceCollection.AddSingleton<AppViewModel>();
        ServiceCollection.AddSingleton<MainWindow>();
        ServiceCollection.AddSingleton<MainWindowViewModel>();
        ServiceCollection.AddSingleton<HomeView>();
        // Http
        ServiceCollection.AddTransient(client => new HttpClient());
        // App upgrade
        ServiceCollection.AddSingleton<IAppUpgradeService>(appUpgrade => new CloudflareAppUpgradeService("https://distributor.krins.cloud",
            "679edd7cffaf4a9ef3be4c445317a461",
            "45fcd2ff239321d48ad4cae7ea9b5c4457f9d12f2483eb4029836931f5f83526",
            "distributor",
            "https://11f33fc072df859ebaf8faa0b0e1766b.r2.cloudflarestorage.com"));

        return ServiceCollection.BuildServiceProvider();
    }
}
