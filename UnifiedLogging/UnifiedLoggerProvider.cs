using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.mahonkin.tim.Logging.UnifiedLogging;

/// <inheritdoc cref="ILoggerProvider"/>
[ProviderAlias("UnifiedLogging")]
public sealed class UnifiedLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, UnifiedLogger> _loggers = new ConcurrentDictionary<string, UnifiedLogger>(StringComparer.OrdinalIgnoreCase);
    private IOptionsMonitor<UnifiedLoggerOptions> _options;
    private readonly IDisposable? _onChangeToken;

    /// <inheritdoc cref="ILoggerProvider"/>
    public UnifiedLoggerProvider(IOptionsMonitor<UnifiedLoggerOptions> options)
    {
        _options = options;
        _onChangeToken = options.OnChange(UpdateOptions);
    }

    /// <inheritdoc cref="ILoggerProvider.CreateLogger(string)"/>
    public ILogger CreateLogger(string category) => _loggers.GetOrAdd(category, name => new UnifiedLogger(category, _options.CurrentValue));

    /// <inheritdoc cref="IDisposable"/>
    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }

    private void UpdateOptions(UnifiedLoggerOptions options)
    {
        throw new NotImplementedException(nameof(UpdateOptions));
    }
}
