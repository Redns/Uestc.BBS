using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Desktop;
using Uestc.BBS.ViewModels;

namespace Uestc.BBS.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            DataContext = App.Services.GetService<MainViewModel>();
            InitializeComponent();
        }
    }
}