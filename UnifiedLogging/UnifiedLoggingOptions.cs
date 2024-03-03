using Microsoft.Extensions.Options;

namespace com.github.timgreynolds.logging.UnifiedLogging;

public class UnifiedLoggerOptions : IOptions<UnifiedLoggerOptions>
{
    public string? Subsystem { get; set; }

    public UnifiedLoggerOptions Value => this;
}
