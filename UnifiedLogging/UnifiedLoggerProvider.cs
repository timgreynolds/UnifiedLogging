using System;
using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.logging;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("UnifiedLogging")]
public sealed class UnifiedLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, UnifiedLogger> _loggers = new ConcurrentDictionary<string, UnifiedLogger>(StringComparer.OrdinalIgnoreCase);

    public ILogger CreateLogger(string category) => _loggers.GetOrAdd(category, new UnifiedLogger(category));

    public void Dispose()
    {
        _loggers.Clear();
    }
}
