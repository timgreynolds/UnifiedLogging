using Microsoft.Extensions.Options;

namespace com.mahonkin.tim.logging.UnifiedLogging;

/// <inheritdoc cref="IOptions{T}"/>
public class UnifiedLoggerOptions : IOptions<UnifiedLoggerOptions>
{
    /// <summary>
    /// String to be used as the application-wide subsystem value.
    /// </summary>
    public string? Subsystem { get; set; }

    /// <inheritdoc cref="IOptions{T}.Value" />
    public UnifiedLoggerOptions Value => this;
}
