using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class HomeViewModel:ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<HomeViewModel> _homes;



    }
}
