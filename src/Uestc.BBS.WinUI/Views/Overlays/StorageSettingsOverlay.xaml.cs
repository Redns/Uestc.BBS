using System.Collections.Generic;
using FastEnumUtility;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Models;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class StorageSettingsOverlay : Page
    {
        private StorageSettingViewModel ViewModel { get; init; }

        private List<SyncMode> SyncModes { get; init; } = [.. FastEnum.GetValues<SyncMode>()];

        public StorageSettingsOverlay(StorageSettingViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }
    }
}
