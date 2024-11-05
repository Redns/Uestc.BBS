using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<MainViewModel>();
    }
}
