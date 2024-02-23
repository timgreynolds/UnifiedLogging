using Microsoft.Extensions.Options;

namespace com.mahonkin.tim.logging.UnifiedLogging;

public class UnifiedLoggerOptions : IOptions<UnifiedLoggerOptions>
{
    public string? Subsystem { get; set; }

    public UnifiedLoggerOptions Value => this;
}
