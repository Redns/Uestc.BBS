using Microsoft.UI.Xaml;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.WinUI.Helpers;
using WinUIEx;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class AuthWindow : WindowEx
    {
        private readonly AppSettingModel _appSettingModel;

        public AuthWindow(AppSettingModel appSettingModel)
        {
            _appSettingModel = appSettingModel;

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
            if (Content is FrameworkElement element)
            {
                element.RequestedTheme = appSettingModel.Apperance.ThemeColor.GetElementTheme();
                appSettingModel.Apperance.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(appSettingModel.Apperance.ThemeColor))
                    {
                        element.RequestedTheme =
                            appSettingModel.Apperance.ThemeColor.GetElementTheme();
                    }
                };
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => Close();
    }
}
