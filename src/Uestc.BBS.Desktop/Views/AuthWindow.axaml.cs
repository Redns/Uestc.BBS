using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Uestc.BBS.Desktop.ViewModels;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Desktop.Views;

public partial class AuthWindow : Window
{

#if DEBUG
    /// <summary>
    /// 仅用于设计器预览
    /// </summary>
    public AuthWindow()
    {
        InitializeComponent();
    }
#endif

    public AuthWindow(AuthViewModel viewModel, AppSettingModel appSettingModel)
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