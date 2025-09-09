using System;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class BrowseSettingOverlay : Page
    {
        private Uri ApiBaseUri { get; init; }

        private AppSettingModel AppSetting { get; init; }

        private BrowseSettingsViewModel ViewModel { get; init; }

        public BrowseSettingOverlay(
            Uri baseUri,
            AppSettingModel appSetting,
            BrowseSettingsViewModel viewModel
        )
        {
            InitializeComponent();

            ApiBaseUri = baseUri;
            ViewModel = viewModel;
            AppSetting = appSetting;
        }

        [RelayCommand]
        private void OpenWebsite(string url) => OperatingSystemHelper.OpenWebsite(url);
    }
}
