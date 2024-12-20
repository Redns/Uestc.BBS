using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Desktop.Helpers;
using Uestc.BBS.Desktop.Models;

namespace Uestc.BBS.Desktop.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        /// <summary>
        /// Copyright
        /// </summary>
        public string Copyright =>
            DateTime.Now.Year == AppHelper.OriginalDatetime.Year
                ? $"©{AppHelper.OriginalDatetime.Year} Redns. MIT License"
                : $"©{AppHelper.OriginalDatetime.Year}-{DateTime.Now.Year} Redns. MIT License";

        [ObservableProperty]
        private AppSettingModel _model;

        private readonly AppSetting _appSetting;

        private readonly ILogService _logService;

        public SettingsViewModel(
            AppSettingModel model,
            AppSetting appSetting,
            ILogService logService
        )
        {
            _model = model;
            _appSetting = appSetting;
            _logService = logService;
        }

        /// <summary>
        /// 主题切换
        /// </summary>
        [RelayCommand]
        private void SwitchTheme() =>
            Application.Current!.RequestedThemeVariant = Model.ThemeColor switch
            {
                ThemeColor.Dark => ThemeVariant.Dark,
                ThemeColor.Light => ThemeVariant.Light,
                _ => ThemeVariant.Default,
            };

        [RelayCommand]
        private void AddUser() { }

        /// <summary>
        /// 打开日志输出路径
        /// </summary>
        [RelayCommand]
        private void OpenLogDirectory()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer.exe", _logService.LogDirectory);
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "explorer",
                        Arguments = "-R " + _logService.LogDirectory,
                        UseShellExecute = true,
                    }
                );
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "xdg-open",
                        Arguments = _logService.LogDirectory,
                        UseShellExecute = true,
                    }
                );
            }
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        [RelayCommand]
        private void ClearLogs()
        {
            if (!Directory.Exists(_logService.LogDirectory))
            {
                return;
            }

            _logService.LogDirectory.DeleteFiles($"*{AppDomain.CurrentDomain.FriendlyName}*.log");

            Model.LogSizeContent =
                $"日志文件存储占用：{_logService.LogDirectory.GetFileTotalSize($"*{AppDomain.CurrentDomain.FriendlyName}*.log").FormatFileSize()}";
        }

        /// <summary>
        /// 保存配置至本地
        /// </summary>
        [RelayCommand]
        private void SaveAppSetting()
        {
            _appSetting.Save();
            Model.LogSizeContent =
                $"日志文件存储占用：{_logService.LogDirectory.GetFileTotalSize($"*{AppDomain.CurrentDomain.FriendlyName}*.log").FormatFileSize()}";
        }
    }
}
