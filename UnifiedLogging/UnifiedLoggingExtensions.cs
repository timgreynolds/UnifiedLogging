using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace com.github.timgreynolds.logging.UnifiedLogging.Extensions;

public static class UnifiedLoggingExtensions
{
    public static ILoggingBuilder AddUnifiedLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, UnifiedLoggerProvider>());
        LoggerProviderOptions.RegisterProviderOptions<UnifiedLoggerOptions, UnifiedLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddUnifiedLogger(this ILoggingBuilder builder, Action<UnifiedLoggerOptions> configure)
    {
        builder.AddUnifiedLogger();
        builder.Services.Configure(configure);

        return builder;
    }
}
