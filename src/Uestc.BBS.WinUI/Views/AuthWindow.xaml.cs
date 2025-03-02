using Microsoft.UI.Xaml;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.WinUI.Helpers;
using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class AuthWindow : WindowEx
    {
        public AuthWindow(AppSettingModel appSettingModel)
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

            // ��������ɫ
            this.SetThemeColor(appSettingModel.Appearance.ThemeColor);
            appSettingModel.Appearance.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(appSettingModel.Appearance.ThemeColor))
                {
                    this.SetThemeColor(appSettingModel.Appearance.ThemeColor);
                }
            };
            App.SystemThemeChanged += (_, args) =>
            {
                AppWindow.TitleBar.SetThemeColor(args);
            };
        }

        private void CloseWindow(object _, RoutedEventArgs e) => Close();
    }
}
