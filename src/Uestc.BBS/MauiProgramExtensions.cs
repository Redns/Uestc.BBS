using CommunityToolkit.Maui;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm;
using Uestc.BBS.ViewModels;
using Uestc.BBS.Views;

namespace Uestc.BBS;

public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder) =>
        builder.UseMauiApp<App>().UseMauiCommunityToolkit().ConfigureCommonServices();

    /// <summary>
    /// 配置移动端公共服务
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder ConfigureCommonServices(this MauiAppBuilder builder)
    {
        ServiceExtension.ConfigureCommonServices();

        ServiceExtension.ServiceCollection.AddTransient<AppShell>();
        ServiceExtension.ServiceCollection.AddTransient<MainPageViewModel>();
        ServiceExtension.ServiceCollection.AddTransient<MainPage>();

        return builder;
    }
}
