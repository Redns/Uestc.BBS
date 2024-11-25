using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Diagnostics;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class AuthWindow : Window
{
    private readonly AppSetting _appSetting;

    public AuthWindow(AuthViewModel viewModel, AppSetting appSetting)
    {
        InitializeComponent();
        DataContext = viewModel; 
        _appSetting = appSetting;
    }

    private void CloseWindow(object? sender, RoutedEventArgs e) => Hide();

    private void DragWindow(object? sender, PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            BeginMoveDrag(e);
        }
    }

    private void OpenOfficialWebsite(object? sender, PointerPressedEventArgs e)
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = _appSetting.Apperance.OfficialUrl,
            UseShellExecute = true
        });
    }
}