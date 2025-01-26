using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Uestc.BBS.WinUI.ViewModels;
using Uestc.BBS.WinUI.Views;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Uestc.BBS.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private readonly AppWindow m_AppWindow;

        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            
            m_AppWindow = GetAppWindowForCurrentWindow();
            m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            m_AppWindow.Resize(new Windows.Graphics.SizeInt32(1500, 900));

            _viewModel = viewModel;

            nvSample.SelectedItem = nvSample.MenuItems[0];
            contentFrame.Navigate(typeof(HomePage));
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
    }
}
