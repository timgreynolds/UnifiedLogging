using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.mahonkin.tim.extensions.Logging;
using com.mahonkin.tim.Logging.OSLog;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.Logging.UnifiedLogging;

/// <summary>
/// <see cref="ILogger"/>Implementation that writes messages to the Apple Unified Logging framework. 
/// </summary>
public sealed class UnifiedLogger(string category, string subsystem, IFormatProvider formatProvider) : ILogger
{
    #region Private Fields
    private readonly IntPtr _logPtr = OSLogger.Create(subsystem, category);
    #endregion Private Fields

    #region Public Methods
    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => OSLogger.IsEnabled(_logPtr, logLevel.LogType());

    /// <inheritdoc/>
    public void Log<T>(LogLevel logLevel, EventId eventId, T state, Exception? exception, Func<T, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        formatter = ValuesFormatter;
        try
        {
            OSLogger.Log(_logPtr, logLevel.LogType(), ValuesFormatter(state, exception));
        }
        catch (Exception ex)
        {
            OSLogger.LogError(_logPtr, "An exception was thrown: {0}", ex.Message);
        }
    }
    #endregion Public Methods

    #region Private Methods
    private string ValuesFormatter<T>(T state, Exception? exception)
    {
        if (exception != null)
        {
            return string.Format(formatProvider, "An exception was thrown: {0} - {1}", exception.GetType().Name, exception.Message);
        }
        // This is the initial message with the value labels. 
        FieldInfo? messageField = state?.GetType().GetField("_originalMessage", BindingFlags.Instance | BindingFlags.NonPublic);
        string message = messageField?.GetValue(state)?.ToString() ?? string.Empty;
        // These are the actual fields being passed. They become the "params" array for String.Format(). 
        // According to the documentation the array should be in the correct order.
        FieldInfo? valuesField = state?.GetType().GetField("_values", BindingFlags.Instance | BindingFlags.NonPublic);
        object[] values = valuesField?.GetValue(state) as object[] ?? [];
        // Here some magic gets done. s_formatters holds the list of placeholder labels for each of the message strings that the state object tracks. The next steps
        // pull out the list for the specific message being processed and, hopefully in proper order, replaces the label with its index number, which is what the 
        // composite formatter expects. Oh, by the way, try to preserve the formatstrings because that's the whole point.
        FieldInfo? formattersField = state?.GetType().GetField("s_formatters", BindingFlags.Static | BindingFlags.NonPublic);
        Dictionary<string, LogValuesEntry>? formatters = JsonSerializer.Deserialize<Dictionary<string, LogValuesEntry>>(JsonSerializer.Serialize(formattersField?.GetValue(state)));

        if (formatters!.TryGetValue(message, out LogValuesEntry? logValues) == false)
        {
            return string.Format(formatProvider, "Could not format log message: '{0}'", message);
        }
        else
        {
            if (logValues.ValueNames.Length != values.Length)
            {
                string exceptionMsg = string.Format(formatProvider, "The number of placeholders in the format string does not match the number of parameters. Placeholders: {0} Paramters: {1}", logValues.ValueNames.Length, values.Length);
                throw new FormatException(exceptionMsg);
            }
            for (int i = 0; i < logValues.ValueNames.Length; i++)
            {
                string stringToReplace = "{" + logValues.ValueNames[i].ToString();
                string replacement = "{" + i.ToString();
                // There's nothing to have stopped someone from re-using label strings, so ValueNames could contain identical strings.
                // Assuming the ValueNames and Values arrays are sequenced appropriately, replace each ValueName in the order in which it is encountered.
                ReplaceFirst(ref message, stringToReplace, replacement);
            }
        }
        formatters.Clear(); // Let's try to make sure this doesn't just grow out of control.
        return string.Format(formatProvider, message, values);
    }

    private void ReplaceFirst(ref string inString, string matchString, string replacementString)
    {
        int index = inString.IndexOf(matchString);
        if (index < 0) return;
        inString = inString.Substring(0, index) + replacementString + inString.Substring(index + matchString.Length);
    }
    #endregion Private Methods

    /// <summary>
    /// Utility class to aid log value formatting.
    /// </summary>
    public class LogValuesEntry
    {
        /// <summary>
        /// The original format string passed in the Ilogger.Log{LogLevel}() method.
        /// </summary>
        [JsonPropertyName("OriginalFormat")]
        public string OriginalFormat { get; set; } = string.Empty;

        /// <summary>
        /// An array containing the "labels" used to identify the variables being formatted in the original string.
        /// </summary>
        [JsonPropertyName("ValueNames")]
        public string[] ValueNames { get; set; } = [];
    }
}