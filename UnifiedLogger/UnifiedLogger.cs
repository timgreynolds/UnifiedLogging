using System;
using CoreFoundation;
using Foundation;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.UnifiedLogger;

public class UnifiedLogger(string name) : ILogger
{
    private CoreFoundation.OSLog _logger = new CoreFoundation.OSLog(NSBundle.MainBundle.BundleIdentifier, name);

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        OSLogLevel osLogLevel = UnifiedLoggerConfig.LogLevelMap[logLevel];
        var message = System.Text.Json.JsonSerializer.Serialize(new 
        {
            EventId = eventId,
            State = state,
            Error = exception
        });

        _logger.Log(osLogLevel, message);
    }
}
