using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Uestc.BBS.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
    }

    /// <summary>
    /// 是否固定窗口至顶部
    /// </summary>
    [ObservableProperty]
    private bool _isWindowPinned = false;

    [ObservableProperty]
    private string _greeting = string.Empty;

    /// <summary>
    /// 导航页面
    /// </summary>
    [ObservableProperty]
    private UserControl _currentPage = App.Services.GetRequiredService<HomeView>();

    /// <summary>
    /// 导航
    /// </summary>
    /// <param name="menu"></param>
    [RelayCommand]
    private void Navigate(string menu)
    {
        //CurrentPage = menu switch
        //{
        //    "Upload" => App.Services.GetRequiredService<UploadView>(),
        //    "Statistics" => App.Services.GetRequiredService<StatisticsView>(),
        //    "Albums" => App.Services.GetRequiredService<AlbumsView>(),
        //    "Plugins" => App.Services.GetRequiredService<PluginsView>(),
        //    "Settings" => App.Services.GetRequiredService<SettingsView>(),
        //    _ => CurrentPage
        //};
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
