using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core;
using Uestc.BBS.WinUI.ViewModels;
using Windows.Graphics;

namespace Uestc.BBS.WinUI
{
    public sealed partial class AuthWindow : Window
    {
        public AuthWindow(AppSetting appSetting, AuthViewModel viewModel)
        {
            InitializeComponent();

            // 内容拓展至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置窗口大小
            AppWindow.Resize(new SizeInt32(480, 400));
            // 隐藏标题栏
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
            // 设置可拖动区域
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, PointerRoutedEventArgs e) => Close();
    }
}
