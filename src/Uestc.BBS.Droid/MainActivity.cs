using Android.App;
using Android.Content.PM;

namespace Uestc.BBS
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        Icon = "@drawable/appicon",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize
            | ConfigChanges.Orientation
            | ConfigChanges.UiMode
            | ConfigChanges.ScreenLayout
            | ConfigChanges.SmallestScreenSize
            | ConfigChanges.Density
    )]
    public class MainActivity : MauiAppCompatActivity { }
}
