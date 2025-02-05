using Microsoft.UI.Xaml.Input;
using WinUIEx;

namespace Uestc.BBS.WinUI
{
    public sealed partial class AuthWindow : WindowEx
    {
        public AuthWindow()
        {
            InitializeComponent();

            // ���ô���λ��
            this.CenterOnScreen();
            // ������չ��������
            ExtendsContentIntoTitleBar = true;
            // ���ÿ��϶�����
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, PointerRoutedEventArgs e) => Close();
    }
}
