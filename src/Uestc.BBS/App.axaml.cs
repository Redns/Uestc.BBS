using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Ke.Bee.Localization.Extensions;
using Ke.Bee.Localization.Options;
using Ke.Bee.Localization.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
        // View & ViewModel
        ServiceCollection.AddSingleton<AppViewModel>();
        ServiceCollection.AddSingleton<MainWindow>();
        ServiceCollection.AddSingleton<MainView>();
        ServiceCollection.AddSingleton<MainViewModel>();
        ServiceCollection.AddSingleton<HomeView>();

        // i18n
        ServiceCollection.AddLocalization<AvaloniaJsonLocalizationProvider>(() =>
        {
            var options = new AvaloniaLocalizationOptions(
                // 支持的本地化语言文化
                [
                    new("en-US"),
                    new("zh-CN")
                ],
                // defaultCulture, 用于设置当前文化（currentCulture）不在 cultures 列表中时的情况以及作为缺失的本地化条目的备用文化（fallback culture）
                new CultureInfo("zh-CN"),
                // currentCulture 在基础设施加载时设置，可以从应用程序设置或其他地方获取
                // FIXME 英文工具栏显示不全
                Thread.CurrentThread.CurrentCulture,
                // 包含本地化 JSON 文件的资源路径
                $"{typeof(App).Namespace}/Assets/i18n");
            return options;
        });

        return ServiceCollection.BuildServiceProvider();
    }
}
