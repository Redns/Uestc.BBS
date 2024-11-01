using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Desktop;
using Uestc.BBS.Desktop.ViewModels;

namespace Uestc.BBS.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            CurrentViewModel = App.Services.GetService<AuthViewModel>();
        }
    }
}
