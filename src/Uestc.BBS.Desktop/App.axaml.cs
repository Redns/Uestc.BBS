using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.System;
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

        try
        {
            var authSetting = ServiceExtension.Services.GetRequiredService<AppSetting>().Auth;
            var isUserAuthed = authSetting.AutoLogin && string.IsNullOrEmpty(authSetting.DefaultCredential?.Token) is false && 
                string.IsNullOrEmpty(authSetting.DefaultCredential?.Token) is false;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                desktop.MainWindow = isUserAuthed ? ServiceExtension.Services.GetService<MainWindow>() : ServiceExtension.Services.GetService<AuthWindow>();
            }
        }
        catch(Exception e)
        {
            ServiceExtension.Services.GetService<ILogService>()?.Error("Application launched failed", e);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
