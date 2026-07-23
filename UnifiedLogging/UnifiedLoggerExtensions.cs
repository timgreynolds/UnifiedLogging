using System;
using com.mahonkin.tim.Logging.UnifiedLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace com.mahonkin.tim.extensions.Logging;

/// <summary>
/// Extension class that implements helper methods to be used with the <see cref="ILoggingBuilder"/>class in the dependency injection container.
/// </summary>
public static class UnifiedLoggingExtensions
{
    /// <summary>
    /// Adds a <see cref="UnifiedLogger"/>to the LoggingBuilder and registers the <see cref="UnifiedLoggerOptions"/>
    /// </summary>
    public static ILoggingBuilder AddUnifiedLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, UnifiedLoggerProvider>());
        LoggerProviderOptions.RegisterProviderOptions<UnifiedLoggerOptions, UnifiedLoggerProvider>(builder.Services);
        return builder;
    }

    /// <summary>
    /// Adds a <see cref="UnifiedLogger"/>to the LoggingBuilder along with a configuration delegate.
    /// </summary>
    public static ILoggingBuilder AddUnifiedLogger(this ILoggingBuilder builder, Action<UnifiedLoggerOptions> configure)
    {
        builder.AddUnifiedLogger().Services.Configure(configure);
        return builder;
    }
}