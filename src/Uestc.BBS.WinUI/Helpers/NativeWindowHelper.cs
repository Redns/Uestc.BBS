﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace Uestc.BBS.WinUI.Helpers
{
    public static class NativeWindowHelper
    {
        private const int WM_NCLBUTTONDBLCLK = 0x00A3; // Non-client left button double-click
        private const int WM_SYSCOMMAND = 0x0112; // System command message
        private const int SC_MAXIMIZE = 0xF030; // Maximize command
        private const int GWLP_WNDPROC = -4;

        // Static field to hold the delegate, preventing it from being garbage-collected
        private static WndProcDelegate _currentWndProcDelegate;

        // Delegate for the new window procedure
        private delegate IntPtr WndProcDelegate(
            IntPtr hwnd,
            uint msg,
            IntPtr wParam,
            IntPtr lParam
        );

        /// <summary>
        /// 禁用窗口最大化功能
        /// </summary>
        /// <param name="window"></param>
        public static void ForceDisableMaximize(this Window window)
        {
            var hwnd = WindowNative.GetWindowHandle(window);

            if (hwnd == IntPtr.Zero)
            {
                Debug.WriteLine("Invalid window handle. Cannot hook window procedure.");
                return;
            }

            // Store the original WndProc and assign the new one
            IntPtr originalWndProc = GetWindowLongPtr(hwnd, GWLP_WNDPROC);
            if (originalWndProc == IntPtr.Zero)
            {
                Debug.WriteLine("Failed to retrieve the original WndProc.");
                return;
            }

            _currentWndProcDelegate = (wndHwnd, msg, wParam, lParam) =>
            {
                // Suppress double-click maximize
                if (msg == WM_NCLBUTTONDBLCLK)
                {
                    Debug.WriteLine("Double-click maximize suppressed.");
                    return IntPtr.Zero;
                }

                // Suppress system maximize command (e.g., via keyboard shortcuts or title bar menu)
                if (msg == WM_SYSCOMMAND && wParam.ToInt32() == SC_MAXIMIZE)
                {
                    Debug.WriteLine("Maximize via system command suppressed.");
                    return IntPtr.Zero;
                }

                try
                {
                    // Ensure parameters are valid before calling originalWndProc
                    if (wndHwnd != IntPtr.Zero && originalWndProc != IntPtr.Zero)
                    {
                        return CallWindowProc(originalWndProc, wndHwnd, msg, wParam, lParam);
                    }
                    else
                    {
                        Debug.WriteLine("Invalid parameters in WndProc call.");
                        return IntPtr.Zero;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions to avoid crashing
                    Debug.WriteLine($"Error in WndProc: {ex.Message}");
                    return IntPtr.Zero;
                }
            };

            try
            {
                // Hook the new WndProc
                IntPtr result = SetWindowLongPtr(
                    hwnd,
                    GWLP_WNDPROC,
                    Marshal.GetFunctionPointerForDelegate(_currentWndProcDelegate)
                );
                if (result == IntPtr.Zero)
                {
                    Debug.WriteLine(
                        $"Failed to set new WndProc. Error: {Marshal.GetLastWin32Error()}"
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error hooking window procedure: {ex.Message}");
                return;
            }

            // Prevent garbage collection of the delegate (redundant but safe)
            GC.KeepAlive(_currentWndProcDelegate);
        }

        // Win32 API declarations
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallWindowProc(
            IntPtr lpPrevWndFunc,
            IntPtr hWnd,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam
        );

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return IntPtr.Size == 8
                ? GetWindowLongPtr64(hWnd, nIndex)
                : GetWindowLong32(hWnd, nIndex);
        }

        private static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return IntPtr.Size == 8
                ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong)
                : SetWindowLong32(hWnd, nIndex, dwNewLong);
        }
    }
}
