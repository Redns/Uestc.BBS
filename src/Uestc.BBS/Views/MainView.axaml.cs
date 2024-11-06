using Avalonia.Controls;
using Avalonia.Interactivity;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Views;

public partial class MainView : UserControl
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var insetsManager = TopLevel.GetTopLevel(this)?.InsetsManager;
        if (insetsManager != null)
        {
            insetsManager.DisplayEdgeToEdge = true;
        }
    }
}
