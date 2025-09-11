using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class BrowseSettingOverlay : Page
    {
        private Uri ApiBaseUri { get; init; }

        private BrowseSettingsViewModel ViewModel { get; init; }

        public BrowseSettingOverlay(Uri baseUri, BrowseSettingsViewModel viewModel)
        {
            InitializeComponent();

            ApiBaseUri = baseUri;
            ViewModel = viewModel;
        }

        private void OpenUserSpace(object sender, PointerRoutedEventArgs _)
        {
            if (sender is not StackPanel stackPanel)
            {
                return;
            }

            if (stackPanel.Tag is not string userSpaceUrl)
            {
                return;
            }

            OperatingSystemHelper.OpenWebsite(userSpaceUrl);
        }
    }
}
