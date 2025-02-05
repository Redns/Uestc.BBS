using System;
using Microsoft.Windows.AppLifecycle;

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
    }
}
