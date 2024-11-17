using CommunityToolkit.Maui;
using Uestc.BBS.Core;

namespace Uestc.BBS;

public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder) => builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureCommonServices();

    /// <summary>
    /// 配置移动端公共服务
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder ConfigureCommonServices(this MauiAppBuilder builder)
	{
        ServiceExtension.ConfigureCommonServices();

        return builder;
	}
}
