using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Diagnostics;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class MainWindow : Window
{
    private readonly AppSetting _appSetting;

    public MainWindow(MainWindowViewModel viewModel, AppSetting appSetting)
    {
        InitializeComponent();
        DataContext = viewModel;
        _appSetting = appSetting;
    }

    private void DoubleTappedResizeWindow(object sender, TappedEventArgs e) =>
        WindowState = WindowState is WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

    private void DragWindow(object? sender, PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            BeginMoveDrag(e);
        }
    }

    private void MinimizeWindow(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void MaximizeWindow(object? sender, RoutedEventArgs e) =>
        WindowState = WindowState is WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

    private void CloseWindow(object? sender, RoutedEventArgs e) => Hide();

    private void OpenOfficialWebsite(object? sender, PointerPressedEventArgs e)
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = _appSetting.Apperance.OfficialUrl,
            UseShellExecute = true
        });
    }
}
