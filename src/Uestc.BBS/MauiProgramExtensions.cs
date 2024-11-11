using CommunityToolkit.Maui;

namespace Uestc.BBS;

public static class MauiProgramExtensions
{
	public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
	{
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit();

		return builder;
	}
}
