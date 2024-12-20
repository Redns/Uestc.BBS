using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Helpers;
using Uestc.BBS.Core.Services;
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

        [ObservableProperty]
        private Task<IEnumerable<Contributor>> _contributors;

        private readonly AppSetting _appSetting;

        private readonly ILogService _logService;

        private readonly IGithubRESTService _githubRESTService;

        public SettingsViewModel(
            AppSettingModel model,
            AppSetting appSetting,
            ILogService logService,
            IGithubRESTService githubRESTService
        )
        {
            _model = model;
            _appSetting = appSetting;
            _logService = logService;
            _githubRESTService = githubRESTService;
            _contributors = githubRESTService
                .GetContributorsAsync("Redns", "Uestc.BBS")
                .ContinueWith(contributors =>
                    contributors.Result.Select(c => new Contributor
                    {
                        Name = c.Login,
                        HomePage = c.HtmlUrl,
                        Avatar = ImageHelper.LoadFromWeb(c.AvatarUrl),
                    })
                );
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

    public class Contributor
    {
        public string Name { get; set; } = string.Empty;

        public string HomePage { get; set; } = string.Empty;

        public Task<Bitmap?> Avatar { get; set; } = Task.FromResult<Bitmap?>(null);
    }
}
