using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.WinUI.Helpers;
using Windows.Graphics;

namespace Uestc.BBS.WinUI
{
    public sealed partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            // 内容拓展至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置窗口大小
            var dpi = this.GetDpi();
            AppWindow.Resize(new SizeInt32(380 * dpi / 96, 320 * dpi / 96));
            // 隐藏标题栏
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
            // 设置可拖动区域
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, PointerRoutedEventArgs e) => Close();
    }
}
