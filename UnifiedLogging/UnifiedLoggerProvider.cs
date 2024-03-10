using System;
using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.mahonkin.tim.logging.UnifiedLogging;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("UnifiedLogging")]
public sealed class UnifiedLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, UnifiedLogger> _loggers = new ConcurrentDictionary<string, UnifiedLogger>(StringComparer.OrdinalIgnoreCase);
    private UnifiedLoggerOptions _currentOptions;
    private readonly IDisposable? _onChangeToken;

    public UnifiedLoggerProvider(IOptionsMonitor<UnifiedLoggerOptions> options)
    {
        _currentOptions = options.CurrentValue;
        _onChangeToken = options.OnChange(updatedOptions => _currentOptions = updatedOptions);
    }

    public ILogger CreateLogger(string category) => _loggers.GetOrAdd(category, name => new UnifiedLogger(category, GetCurrentOptions));

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }

    private UnifiedLoggerOptions GetCurrentOptions() => _currentOptions;
}
