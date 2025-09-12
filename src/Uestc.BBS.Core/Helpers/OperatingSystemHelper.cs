using System.Diagnostics;

namespace Uestc.BBS.Core.Helpers
{
    public static class OperatingSystemHelper
    {
        public static Version? GetAppVersion()
        {
            // FIXME NOT WORK AT ALL
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// 获取操作系统的显示名称
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static string GetOperatingSystemDisplayName()
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

        /// <summary>
        /// 获取系统架构
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
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

        /// <summary>
        /// 打开指定网址
        /// </summary>
        /// <param name="url"></param>
        public static void OpenWebsite(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            Process.Start(new ProcessStartInfo() { FileName = url, UseShellExecute = true });
        }

        /// <summary>
        /// 打开指定目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="autoCreate"></param>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static void OpenDirectory(string directory, bool autoCreate = false)
        {
            if (autoCreate && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (OperatingSystem.IsWindows())
            {
                Process.Start("explorer.exe", directory);
                return;
            }

            if (OperatingSystem.IsMacOS())
            {
                Process.Start("open", $"-R {directory}");
                return;
            }

            if (OperatingSystem.IsLinux())
            {
                Process.Start("xdg-open", directory);
                return;
            }

            throw new PlatformNotSupportedException();
        }
    }
}
