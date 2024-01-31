using System.Collections.Generic;
using CoreFoundation;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.UnifiedLogger;

public static class UnifiedLoggerConfig
{
    public static int EventId { get; } = 0;

    public static Dictionary<LogLevel, OSLogLevel> LogLevelMap { get; } = new Dictionary<LogLevel, OSLogLevel>()
        {
            [LogLevel.Critical] = OSLogLevel.Fault,
            [LogLevel.Error] = OSLogLevel.Error,
            [LogLevel.Debug] = OSLogLevel.Debug,
            [LogLevel.Information] = OSLogLevel.Info,
            [LogLevel.Warning] = OSLogLevel.Info,
            [LogLevel.Trace] = OSLogLevel.Default
        };
}
