using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Desktop.Views;
using Uestc.BBS.Mvvm.Models;

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
        BindingPlugins.DataValidators.Clear();
        DataContext = ServiceExtension.Services.GetService<AppViewModel>();

        try
        {
            var appSetting = ServiceExtension.Services.GetRequiredService<AppSetting>();
            var appSettingModel = ServiceExtension.Services.GetRequiredService<AppSettingModel>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                desktop.MainWindow = false
                    ? null
                    : appSetting.Account.IsUserAuthed
                        ? ServiceExtension.Services.GetRequiredService<MainWindow>()
                        : ServiceExtension.Services.GetRequiredService<AuthWindow>();
                //Current!.ActualThemeVariantChanged += (sender, args) =>
                //    appSettingModel.NotifyTintColorChanged();
                base.OnFrameworkInitializationCompleted();
            }
        }
        catch (Exception e)
        {
            ServiceExtension
                .Services.GetRequiredService<ILogService>()
                .Error("Application launched failed", e);
        }
    }
}
