using System;
using System.IO;
using System.Text;

namespace Uestc.BBS.Desktop.Services.StartupService
{
    public class LinuxStartupService : IStartupService
    {
        private readonly StartupInfo _startupInfo;
        private readonly string _configFilePath = string.Empty;

        public LinuxStartupService(StartupInfo startupInfo)
        {
            _startupInfo = startupInfo;
            _configFilePath = Path.Combine(Environment.GetEnvironmentVariable("HOME")!, ".config/autostart",
                                           $"{_startupInfo.ApplicationName}.desktop");
        }

        public void Disable()
        {
            if (File.Exists(_configFilePath) is false)
            {
                return;
            }
            File.Delete(_configFilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public void Enable()
        {
            if (Directory.Exists(Path.GetDirectoryName(_configFilePath)) is false)
            {
                throw new PlatformNotSupportedException();
            }

            // create config file
            var configContent = new StringBuilder();
            configContent.AppendLine("[Desktop Entry]");
            configContent.AppendLine($"Exec={_startupInfo.ApplicationPath}");
            configContent.AppendLine($"Name={_startupInfo.ApplicationName}");
            configContent.AppendLine("Terminal=False");
            configContent.AppendLine("Type=Application");

            File.WriteAllText(_configFilePath, configContent.ToString());
        }
    }
}
