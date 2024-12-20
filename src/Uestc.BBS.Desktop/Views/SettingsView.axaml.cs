using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using System.Globalization;
using System.Linq.Expressions;
using System;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class SettingsView : UserControl
{
    /// <summary>
    /// �����������Ԥ��
    /// </summary>
    public SettingsView()
    {
        InitializeComponent();
    }

    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}