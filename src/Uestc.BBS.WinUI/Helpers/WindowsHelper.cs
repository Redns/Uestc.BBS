using System;
using System.Security.Principal;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core;

namespace Uestc.BBS.WinUI.Helpers
{
    public static partial class WindowsHelper
    {
        /// <summary>
        /// 重启应用
        /// </summary>
        /// <param name="args">附加参数</param>
        public static void Restart(string? args = null) => AppInstance.Restart(args ?? "restart");

        /// <summary>
        /// 结束应用
        /// </summary>
        public static void Exit() => Environment.Exit(0);

        /// <summary>
        /// 设置主题色
        /// </summary>
        /// <param name="window">当前窗口</param>
        /// <param name="themeColor">主题色</param>
        /// <exception cref="ArgumentException"></exception>
        public static void SetThemeColor(this Window window, ThemeColor themeColor)
        {
            if (window.Content is not FrameworkElement element)
            {
                throw new ArgumentException(
                    "Window content is not FrameworkElement",
                    nameof(window)
                );
            }

            // 设置主题色
            (element.RequestedTheme, window.AppWindow.TitleBar.PreferredTheme) = themeColor switch
            {
                ThemeColor.Light => (ElementTheme.Light, TitleBarTheme.Light),
                ThemeColor.Dark => (ElementTheme.Dark, TitleBarTheme.Dark),
                _ => (ElementTheme.Default, TitleBarTheme.Default),
            };
        }

        public static bool IsAdministartor()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
