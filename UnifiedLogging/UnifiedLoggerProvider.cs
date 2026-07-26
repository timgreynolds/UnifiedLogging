using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using com.mahonkin.tim.Logging.OSLog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.mahonkin.tim.Logging.UnifiedLogging;

/// <inheritdoc cref="ILoggerProvider"/>
[ProviderAlias("UnifiedLogging")]
public sealed class UnifiedLoggerProvider(IOptionsMonitor<UnifiedLoggerOptions> options, IEnumerable<IFormatProvider> formatProviders) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, UnifiedLogger> _loggers = new ConcurrentDictionary<string, UnifiedLogger>(StringComparer.OrdinalIgnoreCase);
    private readonly IFormatProvider _formatter = formatProviders.FirstOrDefault(p => p.GetType() == typeof(OSLogFormatter)) ?? new OSLogFormatter();
    private readonly string _subsystem = options.CurrentValue.Subsystem ?? "UnifiedLogging";

    /// <inheritdoc cref="ILoggerProvider.CreateLogger(string)"/>
    public ILogger CreateLogger(string category) => _loggers.GetOrAdd(category, name => new UnifiedLogger(category, _subsystem, _formatter));

    /// <inheritdoc cref="IDisposable.Dispose()"/>
    public void Dispose() => _loggers.Clear();
}
