using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Uestc.BBS.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// 是否固定窗口至顶部
        /// </summary>
        [ObservableProperty]
        private bool _isWindowPinned = false;

        [ObservableProperty]
        private string _greeting = string.Empty;

        public MainWindowViewModel()
        {
        }

        /// <summary>
        /// 主题切换
        /// </summary>
        [RelayCommand]
        private void SwitchTheme()
        {
            if (Application.Current!.RequestedThemeVariant == ThemeVariant.Default)
            {
                Application.Current.RequestedThemeVariant = Application.Current.PlatformSettings!.GetColorValues().ThemeVariant is Avalonia.Platform.PlatformThemeVariant.Light ?
                    ThemeVariant.Light : ThemeVariant.Dark;
            }
            Application.Current!.RequestedThemeVariant = Application.Current.RequestedThemeVariant == ThemeVariant.Light ? ThemeVariant.Dark : ThemeVariant.Light;
        }

        /// <summary>
        /// 窗口固定切换
        /// </summary>
        [RelayCommand]
        private void SwitchPinWindow() => IsWindowPinned = !IsWindowPinned;
    }
}
