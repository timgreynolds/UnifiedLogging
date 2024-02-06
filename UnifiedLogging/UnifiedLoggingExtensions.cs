
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.UnifiedLogging.Extensions;

public static class UnifiedLoggerExtensions
{
    public static ILoggingBuilder AddUnifiedLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, UnifiedLoggerProvider>());
        return builder;
    }

    // public static OSLogLevel MapOSLogLevel(this LogLevel logLevel)
    // {
    //     switch (logLevel)
    //     {
    //         case LogLevel.Critical: return OSLogLevel.Fault;
    //         case LogLevel.Error: return OSLogLevel.Error;
    //         case LogLevel.Warning: return OSLogLevel.Error;
    //         case LogLevel.Information: return OSLogLevel.Info;
    //         case LogLevel.Debug: return OSLogLevel.Debug;
    //         case LogLevel.Trace: return OSLogLevel.Default;
    //         default: return OSLogLevel.Default;
    //     }
    // }
}
