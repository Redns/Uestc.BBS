using Microsoft.UI.Xaml.Input;
using WinUIEx;

namespace Uestc.BBS.WinUI
{
    public sealed partial class AuthWindow : WindowEx
    {
        public AuthWindow()
        {
            InitializeComponent();

            // 设置窗口位置
            this.CenterOnScreen();
            // 内容拓展至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置可拖动区域
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, PointerRoutedEventArgs e) => Close();
    }
}
