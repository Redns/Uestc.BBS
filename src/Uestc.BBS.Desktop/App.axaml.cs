using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
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
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        DataContext = ServiceExtension.Services.GetService<AppViewModel>();
        var s = ServiceExtension.Services.GetService<AppSetting>();
        // TODO 检查用户是否授权
        var isUserAuthed = false;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            desktop.MainWindow = isUserAuthed ? ServiceExtension.Services.GetRequiredService<MainWindow>() : ServiceExtension.Services.GetRequiredService<AuthWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
