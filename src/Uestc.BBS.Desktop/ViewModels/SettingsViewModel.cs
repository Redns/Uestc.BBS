using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core;
using Uestc.BBS.Desktop.Helpers;
using Uestc.BBS.Desktop.Models;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private AppSettingsModel _model;

        public string Copyright { get; } =
            DateTime.Now.Year == AppHelper.OriginalDatetime.Year
                ? $"© {AppHelper.OriginalDatetime.Year} Redns. MIT License"
                : $"© {AppHelper.OriginalDatetime.Year} - {DateTime.Now.Year} Redns. MIT License";

        public SettingsViewModel(AppSettingsModel model)
        {
            _model = model;
        }
    }
}
