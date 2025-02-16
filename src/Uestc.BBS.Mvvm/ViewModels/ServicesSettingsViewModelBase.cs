using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;

namespace Uestc.BBS.Mvvm.ViewModels
{
    public partial class ServicesSettingsViewModelBase : ObservableObject
    {
        protected ILogService _logService;

        public ServicesSettingModel ServicesSettingModel { get; init; } 

        public ServicesSettingsViewModelBase(
            ILogService logService,
            AppSettingModel appSettingModel
        )
        {
            _logService = logService;
            ServicesSettingModel = appSettingModel.Services;
            ServicesSettingModel.Log.PropertyChanged += (sender, args) =>
            {
                _logService.Setup(appSettingModel.AppSetting.Services.Log);
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
            ServicesSettingModel.Upgrade.LastCheckTime = DateTime.Now;

            await Task.Delay(1000);
        }
    }
}
