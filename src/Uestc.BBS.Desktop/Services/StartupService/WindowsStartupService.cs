using Microsoft.Win32.TaskScheduler;
using System.IO;
using Uestc.BBS.Core.Services;

namespace Uestc.BBS.Desktop.Services.StartupService
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
        public void Disable()
        {
            TaskService.Instance.RootFolder.DeleteTask(_startupInfo.Name, false);
        }

        /// <summary>
        /// 使能自启动服务
        /// </summary>
        /// <exception cref="FileNotFoundException">自启动任务程序不存在</exception>
        public void Enable()
        {
            if (File.Exists(_startupInfo.ApplicationPath) is false)
            {
                throw new FileNotFoundException(nameof(_startupInfo.ApplicationPath));
            }

            var task = TaskService.Instance.FindTask(_startupInfo.Name);
            if (task is not null)
            {
                task.Definition.Settings.Enabled = true;
                task.RegisterChanges();
                return;
            }

            // create new task service
            var taskService = TaskService.Instance.NewTask();

            taskService.Triggers.Add(new BootTrigger());
            taskService.Actions.Add(new ExecAction(_startupInfo.ApplicationPath, workingDirectory: _startupInfo.WorkingDirectory));
            taskService.Settings.Enabled = true;
            taskService.Settings.StopIfGoingOnBatteries = false;
            taskService.Settings.DisallowStartIfOnBatteries = false;
            taskService.Settings.AllowHardTerminate = true;
            taskService.Settings.StartWhenAvailable = true;
            taskService.Settings.WakeToRun = true;
            taskService.Principal.RunLevel = TaskRunLevel.Highest;
            taskService.RegistrationInfo.Description = _startupInfo.Description;

            TaskService.Instance.RootFolder.RegisterTaskDefinition(_startupInfo.Name, taskService);
        }
    }
}