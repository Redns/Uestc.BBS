using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public abstract partial class SettingsViewModelBase : ObservableObject
    {
        protected AppSetting _appSetting;

        protected ILogService _logService;

        protected IStartupService _startupService;

        public Appmanifest Appmanifest { get; init; }

        public AppSettingModel AppSettingModel { get; init; }

        protected SettingsViewModelBase(
            AppSetting appSetting,
            Appmanifest appmanifest,
            AppSettingModel appSettingModel,
            ILogService logService,
            IStartupService startupService
        )
        {
            _appSetting = appSetting;
            _logService = logService;
            _startupService = startupService;

            Appmanifest = appmanifest;
            AppSettingModel = appSettingModel;
            AppSettingModel.Log.PropertyChanged += (sender, args) =>
            {
                _logService.Setup(_appSetting.Log);
            };
            AppSettingModel.Apperance.StartupAndShutdown.PropertyChanged += (sender, args) =>
            {
                if (
                    args.PropertyName
                    != nameof(AppSettingModel.Apperance.StartupAndShutdown.StartupOnLaunch)
                )
                {
                    return;
                }

                if (AppSettingModel.Apperance.StartupAndShutdown.StartupOnLaunch)
                {
                    _startupService.Enable();
                    return;
                }
                _startupService.Disable();
            };
        }

        /// <summary>
        /// 日志文件总大小（字节）
        /// </summary>
        /// <returns></returns>
        public long LogTotalSize =>
            _logService.LogDirectory.GetFileTotalSize(
                $"*{AppDomain.CurrentDomain.FriendlyName}*.log"
            );

        /// <summary>
        /// 打开日志输出路径
        /// </summary>
        [RelayCommand]
        private void OpenLogDirectory()
        {
            if (!Directory.Exists(_logService.LogDirectory))
            {
                Directory.CreateDirectory(_logService.LogDirectory);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer.exe", _logService.LogDirectory);
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("explorer", $"-R {_logService.LogDirectory}");
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", _logService.LogDirectory);
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

            OnPropertyChanged(nameof(LogTotalSize));
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task CheckUpdateAsync()
        {
            AppSettingModel.Upgrade.LastCheckTime = DateTime.Now;

            await Task.Delay(1000);
        }

        [RelayCommand]
        private void OpenWebSite(string url) => OperatingSystemHelper.OpenWebsite(url);
    }
}
