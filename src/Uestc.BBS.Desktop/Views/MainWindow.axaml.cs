using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Desktop.Models;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class MainWindow : Window
{
    private readonly AppSetting _appSetting;

#if DEBUG
    /// <summary>
    /// 仅用于设计器预览
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }
#endif

    public MainWindow(
        MainWindowViewModel viewModel,
        AppSetting appSetting,
        AppSettingModel appSettingModel
    )
    {
        InitializeComponent();
        DataContext = viewModel;
        _appSetting = appSetting;
    }

    private void DoubleTappedResizeWindow(object sender, TappedEventArgs e) =>
        WindowState =
            WindowState is WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

    private void DragWindow(object? sender, PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            BeginMoveDrag(e);
        }
    }

    private void MinimizeWindow(object? sender, RoutedEventArgs e) =>
        WindowState = WindowState.Minimized;

    private void MaximizeWindow(object? sender, RoutedEventArgs e) =>
        WindowState =
            WindowState is WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        //if (_appSetting.Appearance.WindowCloseBehavior == WindowCloseBehavior.Hide)
        //{
        //    Hide();
        //    return;
        //}

        if (
            Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime desktop
        )
        {
            desktop.Shutdown();
        }
    }
}
