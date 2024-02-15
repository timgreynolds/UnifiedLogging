using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.UnifiedLogging;

[ProviderAlias("UnifiedLogger")]
public sealed class UnifiedLoggerProvider : ILoggerProvider
{
    private static readonly ConcurrentDictionary<string, UnifiedLogger> _loggers = new ConcurrentDictionary<string, UnifiedLogger>(StringComparer.OrdinalIgnoreCase);

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, new UnifiedLogger(categoryName));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}