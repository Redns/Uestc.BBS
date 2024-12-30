using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services;

namespace Uestc.BBS.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IDailySentenceService _dailySentenceService;

        [ObservableProperty]
        private string _dailySentence;

        public MainPageViewModel(IDailySentenceService dailySentenceService)
        {
            _dailySentenceService = dailySentenceService;
            _dailySentence = "书中自有黄金屋";
        }
    }
}
