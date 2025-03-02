using System.Collections.Generic;
using System.Linq;
using FastEnumUtility;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.WinUI.ViewModels;

namespace Uestc.BBS.WinUI.Views.Overlays
{
    public sealed partial class AppearanceSettingOverlay : Page
    {
        private AppearanceSettingsViewModel ViewModel { get; init; }

        private List<ThemeColor> ThemeColors { get; init; } = [.. FastEnum.GetValues<ThemeColor>()];

        private List<MenuItemModel> TopBarMenuItems { get; set; }

        private List<MenuItemModel> LeftTopBarMenuItems =>
            [
                .. ViewModel.ApperanceSettingModel.MenuItems.Where(m =>
                    m.Position is Position.LeftTop
                ),
            ];

        private List<MenuItemModel> LeftBottomBarMenuItems =>
            [
                .. ViewModel.ApperanceSettingModel.MenuItems.Where(m =>
                    m.Position is Position.LeftBottom
                ),
            ];

        public AppearanceSettingOverlay(AppearanceSettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

            TopBarMenuItems =
            [
                .. viewModel.ApperanceSettingModel.MenuItems.Where(m => m.Position is Position.Top),
            ];
        }
    }
}
