using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Windows.AppLifecycle;
using Uestc.BBS.Core.Models;
using Windows.Graphics.Imaging;
using Windows.UI;
using WinUIEx;

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

            // 设置窗口主题色
            element.RequestedTheme = themeColor switch
            {
                ThemeColor.Light => ElementTheme.Light,
                ThemeColor.Dark => ElementTheme.Dark,
                _ => ElementTheme.Default,
            };

            // 设置标题栏主题色
            window.AppWindow.TitleBar.SetThemeColor(themeColor);
        }

        /// <summary>
        /// 设置标题栏主题色
        /// </summary>
        /// <param name="titleBar"></param>
        /// <param name="themeColor"></param>
        public static void SetThemeColor(this AppWindowTitleBar titleBar, ThemeColor themeColor)
        {
            switch (themeColor)
            {
                case ThemeColor.Light:
                    titleBar.ButtonForegroundColor = Colors.Black;
                    titleBar.ButtonHoverForegroundColor = Colors.Black;
                    titleBar.ButtonHoverBackgroundColor = Color.FromArgb(0xFF, 0xE5, 0xE5, 0xE5);
                    titleBar.InactiveForegroundColor = Color.FromArgb(0xFF, 0x99, 0x99, 0x99);
                    break;
                case ThemeColor.Dark:
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ButtonHoverForegroundColor = Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Color.FromArgb(0xFF, 0x19, 0x19, 0x19);
                    titleBar.InactiveForegroundColor = Color.FromArgb(0xFF, 0x66, 0x66, 0x66);
                    break;
                default:
                    titleBar.SetThemeColor(App.SystemTheme);
                    break;
            }
        }

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministartor()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 截图控件并保存为图片
        /// </summary>
        /// <param name="control"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task CaptureControlAsImageAsync(this UIElement control, string fileName)
        {
            // 生成 RenderTargetBitmap 对象
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(control);
            // 获取图像的二进制数据
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            // 把图片的二进制数据写入文件存储
            using var fileStream = File.OpenWrite(fileName);
            var encoder = await BitmapEncoder.CreateAsync(
                BitmapEncoder.PngEncoderId,
                fileStream.AsRandomAccessStream()
            );
            var dpi = App.CurrentWindow!.GetDpiForWindow();
            encoder.SetPixelData(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Ignore,
                (uint)renderTargetBitmap.PixelWidth,
                (uint)renderTargetBitmap.PixelHeight,
                dpi,
                dpi,
                pixelBuffer.ToArray()
            );
            await encoder.FlushAsync();
        }

        /// <summary>
        /// 保护窗口内容，详细内容参见 https://learn.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setwindowdisplayaffinity
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwAffinity">0x0：不保护，0x1：获取屏幕内容时窗口变为黑色，0x11：获取屏幕内容时窗口变为透明</param>
        /// <returns></returns>
        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);
    }
}
