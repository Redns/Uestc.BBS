using System;

namespace Uestc.BBS.Core.Helpers
{
    public static class OperatingSystemHelper
    {
        public static Version? GetAppVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static string GetOperatingSystem()
        {
            if (OperatingSystem.IsWindows())
            {
                return "windows";
            }
            else if (OperatingSystem.IsLinux())
            {
                return "linux";
            }
            else if (OperatingSystem.IsMacOS())
            {
                return "macOS";
            }
            else if (OperatingSystem.IsAndroid())
            {
                return "android";
            }
            else if (OperatingSystem.IsIOS())
            {
                return "ios";
            }
            throw new PlatformNotSupportedException();
        }

        public static string GetSystemArchitecture()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "AMD64")
                {
                    return "x64";
                }
                else if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "ARM64")
                {
                    return "arm64";
                }
                throw new PlatformNotSupportedException();
            }
            return "x86";
        }
    }
}
