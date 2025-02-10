using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core;

namespace Uestc.BBS.WinUI.Helpers
{
    public static partial class WindowsHelper
    {
        /// <summary>
        /// 请求管理员权限
        /// </summary>
        /// <param name="window"></param>
        /// <param name="contentDialog"></param>
        /// <returns></returns>
        public static async Task RequireAdministratorPermissionAsync(
            this Window window,
            ContentDialog? contentDialog = null
        )
        {
            contentDialog ??= new ContentDialog
            {
                XamlRoot = window.Content.XamlRoot,
                Title = "自启动",
                PrimaryButtonText = "确 定",
                CloseButtonText = "取 消",
                DefaultButton = ContentDialogButton.Primary,
                Content = "更改该设置需要管理员权限，现在重启应用？",
            };

            var result = await contentDialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            // 以管理员身份重启应用
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = Environment.ProcessPath,
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    Verb = "runas",
                }
            );
            Exit();
        }

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
