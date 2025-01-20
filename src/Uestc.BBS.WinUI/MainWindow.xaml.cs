using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using Uestc.BBS.Core.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Uestc.BBS.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private readonly IDailySentenceService _dailySentenceService;
        public MainWindow(IDailySentenceService dailySentenceService)
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;

            _dailySentenceService = dailySentenceService;
        }
    }
}
