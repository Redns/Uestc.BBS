using UIKit;

namespace Uestc.BBS.iOS;

public class Application
{
    // This is the main entry point of the application.
    public static void Main(string[] args)
    {
        ConfigureServices();

        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }

    private static void ConfigureServices()
    {

    }
}
