using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.ViewModels;
using Uestc.BBS.Views;

namespace Uestc.BBS;

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
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = Services.GetService<MainView>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// 注入公共依赖
    /// </summary>
    /// <returns></returns>
    private static ServiceProvider ConfigureServices()
    {
        ServiceCollection.AddSingleton<AppViewModel>();
        ServiceCollection.AddSingleton<MainWindow>();
        ServiceCollection.AddSingleton<MainView>();
        ServiceCollection.AddSingleton<MainViewModel>();
        ServiceCollection.AddSingleton<HomeView>();

        return ServiceCollection.BuildServiceProvider();
    }
}
