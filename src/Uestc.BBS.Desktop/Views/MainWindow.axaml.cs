using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class MainWindow : Window
{
    private readonly AppSetting _appSetting;

    /// <summary>
    /// 仅用于设计器预览
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel viewModel, AppSetting appSetting)
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
        if (_appSetting.Apperance.WindowCloseBehavior == WindowCloseBehavior.Hide)
        {
            Hide();
            return;
        }

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
        {
            application.Shutdown();
        }
    }
}
