using banditoth.MAUI.DeviceId;
using CommunityToolkit.Maui;
using Uestc.BBS.Core;

namespace Uestc.BBS;

public static class MauiProgramExtensions
{
	public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
	{
		ServiceExtension.ConfigureServices(s => { });
		
        builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureDeviceIdProvider();

		return builder;
	}
}
