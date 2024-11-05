using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace Uestc.BBS.Android;

[Activity(
    Label = "清水河畔",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        ConfigureServices();
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }

    private static void ConfigureServices()
    {

    }
}
