using Uestc.BBS.Core;
using Setting = Android.Provider.Settings;
using static Android.Provider.Settings;
using AndroidApp = Android.App.Application;

namespace Uestc.BBS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Device id
            ServiceExtension.ServiceCollection.AddSingleton(deviceId => Secure.GetString(AndroidApp.Context.ContentResolver, Secure.AndroidId) ?? string.Empty);

            return MauiApp.CreateBuilder()
                .UseSharedMauiApp()
                .Build();
        }
    }
}
