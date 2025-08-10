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
            //// 禁用交互
            //CacheStatisticsPlot.UserInputProcessor.IsEnabled = false;
            //// 设置透明背景
            //CacheStatisticsPlot.Plot.FigureBackground = new BackgroundStyle
            //{
            //    Color = Color.FromARGB(0),
            //};
            //CacheStatisticsPlot.Refresh();
        }
    }
}
