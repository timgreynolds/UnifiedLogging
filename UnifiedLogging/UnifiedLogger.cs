using System;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.logging.UnifiedLogging;

public class UnifiedLogger : ILogger
{
    private IntPtr _logPtr;

    public UnifiedLogger(string category, Func<UnifiedLoggerOptions> getCurrentOptions)
    {
        _logPtr = OSLogger.Create(getCurrentOptions().Subsystem ?? string.Empty, category);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
    public bool IsEnabled(LogLevel logLevel) => OSLogger.IsEnabled(_logPtr, OSLogger.GetOsLogType(logLevel));

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        OSLogger.Log(_logPtr, OSLogger.GetOsLogType(logLevel), formatter(state, exception));
        if (logLevel == LogLevel.Error)
        {
            OSLogger.LogError(_logPtr, formatter(state, exception));
        }
        if (logLevel == LogLevel.Critical)
        {
            OSLogger.LogCritical(_logPtr, formatter(state, exception));
        }
    }
}
