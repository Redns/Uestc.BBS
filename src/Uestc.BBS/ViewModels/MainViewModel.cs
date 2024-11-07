using Avalonia.Controls;
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
}
