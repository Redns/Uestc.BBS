using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class AuthWindow : Window
{
    /// <summary>
    /// �����������Ԥ��
    /// </summary>
    public AuthWindow()
    {
        InitializeComponent();
    }

    public AuthWindow(AuthViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel; 
    }

    private void CloseWindow(object? sender, RoutedEventArgs e) => Hide();

    private void DragWindow(object? sender, PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            BeginMoveDrag(e);
        }
    }
}