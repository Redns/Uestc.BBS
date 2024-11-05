using Avalonia.Controls;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContext = App.Services.GetService<MainViewModel>();
    }
}
