using System;
using System.Diagnostics;
using System.IO;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32.TaskScheduler;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.WinUI.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Uestc.BBS.WinUI.Services
{
    /// <summary>
    /// Windows 自启动服务，需要管理员权限
    /// </summary>
    public class WindowsStartupService(StartupInfo startupInfo) : IStartupService
    {
        private readonly StartupInfo _startupInfo = startupInfo;

        /// <summary>
        /// 禁用自启动服务
        /// </summary>
        public bool Disable()
        {
            // 检查是否具有管理员权限
            if (WindowsHelper.IsAdministartor())
            {
                TaskService.Instance.RootFolder.DeleteTask(_startupInfo.Name, false);
                return true;
            }

            // 请求重新以管理员权限运行
            _ = RequireAdministratorPermissionAsync();

            return false;
        }

        /// <summary>
        /// 使能自启动服务
        /// </summary>
        /// <exception cref="FileNotFoundException">自启动任务程序不存在</exception>
        public bool Enable()
        {
            // 确保 exe 文件存在
            if (File.Exists(_startupInfo.ApplicationPath) is false)
            {
                throw new FileNotFoundException(nameof(_startupInfo.ApplicationPath));
            }

            // 检查是否具有管理员权限
            if (!WindowsHelper.IsAdministartor())
            {
                // 请求重新以管理员权限运行
                _ = RequireAdministratorPermissionAsync();
                return false;
            }

            // 检查是否已存在计划任务
            var task = TaskService.Instance.FindTask(_startupInfo.Name);
            if (task is not null)
            {
                task.Definition.Settings.Enabled = true;
                task.RegisterChanges();
                return task.Enabled;
            }

            // 创建计划任务
            var taskService = TaskService.Instance.NewTask();

            taskService.Triggers.Add(new BootTrigger());
            taskService.Actions.Add(
                new ExecAction(
                    _startupInfo.ApplicationPath,
                    workingDirectory: _startupInfo.WorkingDirectory
                )
            );
            taskService.Settings.Enabled = true;
            taskService.Settings.StopIfGoingOnBatteries = false;
            taskService.Settings.DisallowStartIfOnBatteries = false;
            taskService.Settings.AllowHardTerminate = true;
            taskService.Settings.StartWhenAvailable = true;
            taskService.Settings.WakeToRun = true;
            taskService.Principal.RunLevel = TaskRunLevel.Highest;
            taskService.RegistrationInfo.Description = _startupInfo.Description;

            return TaskService
                .Instance.RootFolder.RegisterTaskDefinition(_startupInfo.Name, taskService)
                .Enabled;
        }

        private async Task RequireAdministratorPermissionAsync()
        {
            var result = await new ContentDialog
            {
                XamlRoot = App.CurrentWindow!.Content.XamlRoot,
                Title = "自启动",
                PrimaryButtonText = "确 定",
                CloseButtonText = "取 消",
                DefaultButton = ContentDialogButton.Primary,
                Content = "更改该设置需要管理员权限，现在重启应用？",
            }.ShowAsync();

            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            // 以管理员身份重启应用
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = Environment.ProcessPath,
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    Verb = "runas",
                }
            );
            WindowsHelper.Exit();
        }
    }
}
