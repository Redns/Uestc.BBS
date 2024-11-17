using CommunityToolkit.Maui;
using NLog;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services;

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
        ServiceExtension.ServiceCollection.AddSingleton<ILogService>(log =>
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;
            var logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "logs", $"{appName}.log");
            var logger = LogManager.Setup().RegisterMauiLog().LoadConfiguration(builder => {
                builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToFile(
                    fileName: logFilePath,
                    encoding: System.Text.Encoding.UTF8,
                    archiveAboveSize: 100 * 1024,
                    layout: "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [Uestc.BBS ${uppercase:${level}}] ${message}${newline}${exception:format=toString}");
            })
            .GetCurrentClassLogger();
            return new NLogService(logger);
        });

        return builder;
	}
}
