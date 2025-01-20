using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Uestc.BBS.Core.Services;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Uestc.BBS.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;

        private readonly IDailySentenceService _dailySentenceService;
        public MainWindow(IDailySentenceService dailySentenceService)
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;

            m_AppWindow = GetAppWindowForCurrentWindow();
            m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

            _dailySentenceService = dailySentenceService;
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
    }
}
