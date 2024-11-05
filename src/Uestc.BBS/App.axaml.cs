using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.ViewModels;
using Uestc.BBS.Views;

namespace Uestc.BBS;

public partial class App : Application
{
    public static readonly ServiceCollection ServiceCollection = new();
    public static ServiceProvider Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 初始化服务
        Services = ConfigureServices();

        // 初始化系统主题色
        if (Current!.RequestedThemeVariant == ThemeVariant.Default)
        {
            Current!.RequestedThemeVariant = Current!.PlatformSettings!.GetColorValues().ThemeVariant is PlatformThemeVariant.Light ?
                ThemeVariant.Light : ThemeVariant.Dark;
        }

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        DataContext = Services.GetRequiredService<AppViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = Services.GetRequiredService<MainView>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// 初始化服务项
    /// </summary>
    /// <returns></returns>
    private static ServiceProvider ConfigureServices()
    {
        // Views & ViewModels
        ServiceCollection.AddSingleton<AppViewModel>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainView>()
                .AddSingleton<MainViewModel>();

        return ServiceCollection.BuildServiceProvider();
    }
}
