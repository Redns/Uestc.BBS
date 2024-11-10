using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Uestc.BBS;

public static class MauiProgramExtensions
{
	public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
	{
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("HarmonyOS_Sans_SC_Regular.ttf", "HarmonyOSRegular");
				fonts.AddFont("HarmonyOS_Sans_SC_Bold.ttf", "HarmonyOSBold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder;
	}
}
