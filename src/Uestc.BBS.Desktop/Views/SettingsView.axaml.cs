using Avalonia.Controls;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class SettingsView : UserControl
{

#if DEBUG
    /// <summary>
    /// �����������Ԥ��
    /// </summary>
    public SettingsView()
    {
        InitializeComponent();
    }
#endif

    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}