using System;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.Helpers;
using Uestc.BBS.Desktop.Models;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private AppSettingsModel _model;

        [ObservableProperty]
        private string _copyright;

        private readonly AppSetting _appSetting;

        public SettingsViewModel(AppSettingsModel model, AppSetting appSetting)
        {
            _model = model;
            _copyright =
                DateTime.Now.Year == AppHelper.OriginalDatetime.Year
                    ? $"©{AppHelper.OriginalDatetime.Year} Redns. MIT License"
                    : $"©{AppHelper.OriginalDatetime.Year}-{DateTime.Now.Year} Redns. MIT License";
            _appSetting = appSetting;
        }

        /// <summary>
        /// 主题切换
        /// </summary>
        [RelayCommand]
        private void SwitchTheme() => Application.Current!.RequestedThemeVariant = Model.ThemeColor switch
        {
            ThemeColor.Dark => ThemeVariant.Dark,
            ThemeColor.Light => ThemeVariant.Light,
            _ => ThemeVariant.Default
        };

        [RelayCommand]
        private void SaveAppSetting()
        {
            _appSetting.Save();
        }
    }
}
