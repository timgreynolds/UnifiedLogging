using System;
using System.Reflection;
using com.mahonkin.tim.extensions.Logging;
using com.mahonkin.tim.logging;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.logging.UnifiedLogging;

/// <summary>
/// <see cref="ILogger"/>Implementation that writes messages to the Apple Unified Logging framework. 
/// </summary>
public class UnifiedLogger : ILogger
{
    #region Private Fields
    private readonly IntPtr _logPtr;
    #endregion Private Fields

    #region Constructors
    /// <inheritdoc cref="ILogger"/>
    public UnifiedLogger(string category, Func<UnifiedLoggerOptions> getCurrentOptions)
    {
        string? subsystem = string.IsNullOrEmpty(getCurrentOptions().Subsystem) ? Assembly.GetExecutingAssembly().GetName().Name : getCurrentOptions().Subsystem;
        _logPtr = OSLogger.Create(subsystem ?? "UnifiedLogger", category);
    }
    #endregion Constructors

    #region Public Methods

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        OSLogger.Log(_logPtr, logLevel.LogType(), formatter(state, exception));
    }
    #endregion Public Methods
}
