using Microsoft.Extensions.Options;

namespace com.mahonkin.tim.logging;

public class UnifiedLoggingOptions : IOptions<UnifiedLoggingOptions>
{
    public string Subsystem { get; set; } = string.Empty;

    public UnifiedLoggingOptions Value => this;
}
