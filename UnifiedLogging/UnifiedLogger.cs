using System;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.logging;

public class UnifiedLogger(string category) : ILogger
{
    private static readonly string _subsystem = System.Reflection.Assembly.GetCallingAssembly().GetName().Name ?? typeof(UnifiedLogger).Name;

    private IntPtr _logPtr = OSLog.Create(_subsystem, category);

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
    public bool IsEnabled(LogLevel logLevel) => OSLog.IsEnabled(_logPtr, OSLog.GetOSLogType(logLevel));

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        OSLog.Log(_logPtr, OSLog.GetOSLogType(logLevel), formatter(state, exception));
        if (logLevel == LogLevel.Error)
        {
            OSLog.LogError(_logPtr, formatter(state, exception));
        }
        if (logLevel == LogLevel.Critical)
        {
            OSLog.LogCritical(_logPtr, formatter(state, exception));
        }
    }
}
