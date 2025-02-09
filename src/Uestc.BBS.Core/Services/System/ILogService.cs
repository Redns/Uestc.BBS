using FastEnumUtility;

namespace Uestc.BBS.Core.Services.System
{
    public interface ILogService
    {
        string LogDirectory { get; }

        void Setup(LogSetting setting);

        void Trace(string message);

        void Trace(string message, Exception exception);

        void Debug(string message);

        void Debug(string message, Exception exception);

        void Info(string message);

        void Info(string message, Exception exception);

        void Warn(string message);

        void Warn(string message, Exception exception);

        void Error(string message);

        void Error(string message, Exception exception);

        void Fatal(string message);

        void Fatal(string message, Exception exception);
    }

    public enum LogLevel
    {
        [Label("Trace")]
        Trace,

        [Label("Debug")]
        Debug,

        [Label("Info")]
        Info,

        [Label("Warning")]
        Warn,

        [Label("Error")]
        Error,

        [Label("Fatal")]
        Fatal
    }
}
