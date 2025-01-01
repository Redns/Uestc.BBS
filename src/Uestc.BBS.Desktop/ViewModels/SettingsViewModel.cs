using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Models;

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
        private bool _isCheckUpgrading = false;

        [ObservableProperty]
        private AppSettingModel _model = model;

        [ObservableProperty]
        private IEnumerable<Contributor> _contributors = appmanifest.Contributors ?? [];

        /// <summary>
        /// 主题切换
        /// </summary>
        [RelayCommand]
        private void SetThemeColor() =>
            Application.Current!.RequestedThemeVariant = Model.ThemeColor switch
            {
                ThemeColor.Dark => ThemeVariant.Dark,
                ThemeColor.Light => ThemeVariant.Light,
                _ => ThemeVariant.Default,
            };

        /// <summary>
        /// 添加用户
        /// </summary>
        [RelayCommand]
        private void AddUser() { }

        /// <summary>
        /// 打开日志输出路径
        /// </summary>
        [RelayCommand]
        private void OpenLogDirectory()
        {
            if (!Directory.Exists(logService.LogDirectory))
            {
                Directory.CreateDirectory(logService.LogDirectory);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer.exe", logService.LogDirectory);
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("explorer", $"-R {logService.LogDirectory}");
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", logService.LogDirectory);
            }
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        [RelayCommand]
        private void ClearLogs()
        {
            if (!Directory.Exists(logService.LogDirectory))
            {
                return;
            }

            logService.LogDirectory.DeleteFiles($"*{AppDomain.CurrentDomain.FriendlyName}*.log");

            Model.LogSizeContent =
                $"日志文件存储占用：{logService.LogDirectory.GetFileTotalSize($"*{AppDomain.CurrentDomain.FriendlyName}*.log").FormatFileSize()}";
        }

        [RelayCommand]
        private async Task CheckUpgradeAsync()
        {
            IsCheckUpgrading = true;

            await Task.Delay(1000);

            Model.LastUpgradeCheckTime = DateTime.Now;

            IsCheckUpgrading = false;
        }

        /// <summary>
        /// 保存配置至本地
        /// </summary>
        [RelayCommand]
        private void SaveAppSetting()
        {
            appSetting.Save();
            Model.LogSizeContent =
                $"日志文件存储占用：{logService.LogDirectory.GetFileTotalSize($"*{AppDomain.CurrentDomain.FriendlyName}*.log").FormatFileSize()}";
        }

        [RelayCommand]
        private void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo() { FileName = url, UseShellExecute = true });
        }
    }
}
