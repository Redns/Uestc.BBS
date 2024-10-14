using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

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
