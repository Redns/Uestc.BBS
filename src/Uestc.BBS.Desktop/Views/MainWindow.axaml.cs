using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Diagnostics;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
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
}
