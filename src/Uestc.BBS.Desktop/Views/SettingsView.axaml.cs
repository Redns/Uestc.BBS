using Avalonia.Controls;
using System;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        // XXX 仅用于设计器预览
        InitializeComponent();
    }

    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}