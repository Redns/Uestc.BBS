using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class ApiSettingOverlay : Page
    {
        private ApiSettingModel Model { get; init; }

        public ApiSettingOverlay(AppSettingModel model)
        {
            InitializeComponent();

            Model = model.Api;
        }
    }
}
