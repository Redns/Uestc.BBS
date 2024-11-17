using Uestc.BBS.Core;
using UIKit;

namespace Uestc.BBS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp() => MauiApp.CreateBuilder()
            .UseSharedMauiApp()
            .ConfigureServices()
            .Build();

        /// <summary>
        /// 配置 iOS 平台服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
        {
            // Device id
            ServiceExtension.ServiceCollection.AddSingleton(deviceId => UIDevice.CurrentDevice.IdentifierForVendor.ToString());

            return builder;
        }
    }
}
