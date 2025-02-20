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

            // 设置窗口位置
            this.CenterOnScreen();
            // 设置窗口图标
            this.SetIcon("Assets/Icons/app.ico");
            // 内容拓展至标题栏
            ExtendsContentIntoTitleBar = true;
            // 设置可拖动区域
            SetTitleBar(AppTitleBar);

            // 设置主题色
            this.SetThemeColor(appSettingModel.Appearance.ThemeColor);
            appSettingModel.Appearance.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(appSettingModel.Appearance.ThemeColor))
                {
                    this.SetThemeColor(appSettingModel.Appearance.ThemeColor);
                }
            };
            App.SystemThemeChanged += (sender, args) =>
            {
                AppWindow.TitleBar.SetThemeColor(args);
            };
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => Close();
    }
}
