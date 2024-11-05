using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Uestc.BBS.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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

    private void CloseWindow(object? sender, RoutedEventArgs e) => Close();

    /// <summary>
    /// 关闭窗口时阻止应用退出
    /// </summary>
    /// <param name="e"></param>
    protected override void OnClosing(WindowClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}
