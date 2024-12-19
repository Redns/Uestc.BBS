using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Uestc.BBS.Core.Services.System;

namespace Uestc.BBS.Core.Services.System
{
    public class NLogService(Logger logger) : ILogService
    {
        private readonly Logger _logger = logger;

        public void Setup(LogSetting setting)
        {
            if (!setting.IsEnable)
            {
                return;
            }

            var config = new LoggingConfiguration();
            var appName = AppDomain.CurrentDomain.FriendlyName;
            var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{appName}/Logs/{appName}.log");
            config.AddRule(GetLogLevel(setting.MinLevel), NLog.LogLevel.Fatal, new AsyncTargetWrapper(new FileTarget()
            {
                FileName = outputPath,
                Layout = setting.OutputFormat,
                KeepFileOpen = false,
                ArchiveAboveSize = 10 * 1024 * 1024,
                MaxArchiveFiles = 10
            }));

            LogManager.Configuration = config;
            LogManager.ReconfigExistingLoggers();
        }

        public static NLog.LogLevel GetLogLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => NLog.LogLevel.Trace,
                LogLevel.Debug => NLog.LogLevel.Debug,
                LogLevel.Info => NLog.LogLevel.Info,
                LogLevel.Warn => NLog.LogLevel.Warn,
                LogLevel.Error => NLog.LogLevel.Error,
                LogLevel.Fatal => NLog.LogLevel.Fatal,
                _ => NLog.LogLevel.Info,
            };
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            _logger.Debug(exception, message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            _logger.Info(exception, message);
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Trace(string message, Exception exception)
        {
            _logger.Trace(exception, message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            _logger.Warn(exception, message);
        }
    }
}
