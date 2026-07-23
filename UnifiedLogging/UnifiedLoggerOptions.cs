using Microsoft.Extensions.Options;

namespace com.mahonkin.tim.Logging.UnifiedLogging;

/// <summary>
/// Options for the Unified Logging logger.
/// </summary>
public sealed class UnifiedLoggerOptions : IOptions<UnifiedLoggerOptions>
{
    /// <summary>
    /// String to be used as the application-wide subsystem value.
    /// </summary>
    public string? Subsystem { get; set; }

    /// <summary>
    /// Gets the configured UnifiedLoggerOptions instance.
    /// </summary>
    public UnifiedLoggerOptions Value => this;
}