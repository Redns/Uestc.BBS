using System.Collections.Generic;
using FastEnumUtility;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Models;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class StorageSettingOverlay : Page
    {
        private StorageSettingViewModel ViewModel { get; init; }

        private List<SyncMode> SyncModes { get; init; } = [.. FastEnum.GetValues<SyncMode>()];

        public StorageSettingOverlay(StorageSettingViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

            //CacheStatisticsPlot.Plot.Add.Signal(Generate.Sin(51));
            //// ���ý���
            //CacheStatisticsPlot.UserInputProcessor.IsEnabled = false;
            //// ����͸������
            //CacheStatisticsPlot.Plot.FigureBackground = new BackgroundStyle
            //{
            //    Color = Color.FromARGB(0),
            //};
            //CacheStatisticsPlot.Refresh();
        }
    }
}
