using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class AuthWindow : WindowEx
    {
        public AuthWindow()
        {
            InitializeComponent();

            // 设置窗口位置
            this.CenterOnScreen();
            // 设置窗口图标
            this.SetIcon("Assets/Icons/app.ico");
            // 内容拓展至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置可拖动区域
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) => Close();
    }
}
