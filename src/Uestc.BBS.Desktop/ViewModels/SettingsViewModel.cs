using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Models;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class SettingsViewModel(
        AppSettingModel model,
        AppSetting appSetting,
        HttpClient httpClient,
        Appmanifest appmanifest,
        ILogService logService
    ) : ObservableObject
    {
        public string AppVersion => appmanifest.Version;

        /// <summary>
        /// Copyright
        /// </summary>
        public string Copyright =>
            DateTime.Now.Year == appmanifest.OriginalDate.Year
                ? $"©{appmanifest.OriginalDate.Year} Redns. MIT License"
                : $"©{appmanifest.OriginalDate.Year}-{DateTime.Now.Year} Redns. MIT License";

        [ObservableProperty]
        public partial bool IsCheckUpgrading { get; set; } = false;

        [ObservableProperty]
        public partial AppSettingModel Model { get; set; }

        [ObservableProperty]
        private IEnumerable<Contributor> _contributors = appmanifest.Contributors ?? [];

        /// <summary>
        /// 主题切换
        /// </summary>
        [RelayCommand]
        private void SetThemeColor() { }

        //Application.Current!.RequestedThemeVariant = Model.ThemeColor switch
        //{
        //    ThemeColor.Dark => ThemeVariant.Dark,
        //    ThemeColor.Light => ThemeVariant.Light,
        //    _ => ThemeVariant.Default,
        //};

    }
}
