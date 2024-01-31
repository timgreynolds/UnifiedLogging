using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.UnifiedLogger.Extensions;

public static class UnifiedLoggerExtensions
{
    public static ILoggingBuilder AddUnifiedLogger(this ILoggingBuilder builder) 
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, UnifiedLoggerProvider>());
        return builder;
    } 
}
