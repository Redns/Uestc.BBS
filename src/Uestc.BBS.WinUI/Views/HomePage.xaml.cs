using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel { get; init; }

        private TopicContent TopicContent { get; set; } =
            new()
            {
                Information = "Hello World",
                Type = TopicContenType.InlineLink,
                Url = "https://www.baidu.com",
            };

        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
