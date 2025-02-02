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

            // ������չ��������
            ExtendsContentIntoTitleBar = true;
            // ���ô��ڴ�С
            AppWindow.Resize(new SizeInt32(480, 400));
            // ���ر�����
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
            // ���ÿ��϶�����
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, PointerRoutedEventArgs e) => Close();
    }
}
