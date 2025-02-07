using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Mvvm.ViewModels;
using Windows.System;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class SettingsPageViewModel : SettingsViewModel
    {
        [RelayCommand]
        public override async Task CheckUpdateAsync()
        {
            // 检查是否有最新版

            // 如果为 UnPackaged 版本
            // 前往镜像源下载最新版本安装包

            // 如果为 Package 版本
            // 跳转至 Microsoft Store 应用页面更新
            await Launcher.LaunchUriAsync(
                new Uri("ms-windows-store://pdp/?productid=9NQDW009T0T5")
            );
        }
    }
}
