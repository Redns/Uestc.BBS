using System;
using System.Runtime.InteropServices;

namespace Uestc.BBS.WinUI.Helpers
{
    public static partial class WindowsHelper
    {

        [LibraryImport("User32.dll", SetLastError = true)]
        public static partial int GetDpiForWindow(IntPtr hwnd);

        public static int GetDpi(this object target) =>
            GetDpiForWindow(WinRT.Interop.WindowNative.GetWindowHandle(target));
    }
}
