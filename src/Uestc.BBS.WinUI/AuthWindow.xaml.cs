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

            // ������չ��������
            ExtendsContentIntoTitleBar = true;
            // ���ô��ڴ�С
            var dpi = this.GetDpi();
            AppWindow.Resize(new SizeInt32(380 * dpi / 96, 320 * dpi / 96));
            // ���ر�����
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
            // ���ÿ��϶�����
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, PointerRoutedEventArgs e) => Close();
    }
}
