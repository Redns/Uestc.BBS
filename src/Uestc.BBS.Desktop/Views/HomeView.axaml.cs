using Avalonia.Controls;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.Desktop.Views;

public partial class HomeView : UserControl
{

#if DEBUG
    public HomeView()
    {
        InitializeComponent();
    }
#endif

    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}