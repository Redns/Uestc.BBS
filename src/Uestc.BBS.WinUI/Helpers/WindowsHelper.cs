using System;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core;

namespace Uestc.BBS.WinUI.Helpers
{
    public static partial class WindowsHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public static void Restart(string? args = null) => AppInstance.Restart(args ?? "restart");

        /// <summary>
        ///
        /// </summary>
        public static void Exit() => Environment.Exit(0);

        public static ElementTheme GetElementTheme(this ThemeColor themeColor) =>
            themeColor switch
            {
                ThemeColor.Light => ElementTheme.Light,
                ThemeColor.Dark => ElementTheme.Dark,
                _ => ElementTheme.Default,
            };
    }
}
