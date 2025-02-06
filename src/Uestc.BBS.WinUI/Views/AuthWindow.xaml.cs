using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class AuthWindow : WindowEx
    {
        public AuthWindow()
        {
            InitializeComponent();

            // ���ô���λ��
            this.CenterOnScreen();
            // ���ô���ͼ��
            this.SetIcon("Assets/Icons/app.ico");
            // ������չ��������
            ExtendsContentIntoTitleBar = true;
            // ���ÿ��϶�����
            SetTitleBar(AppTitleBar);
        }

        private void CloseWindow(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) => Close();
    }
}
