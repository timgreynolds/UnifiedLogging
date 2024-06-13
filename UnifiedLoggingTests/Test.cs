using com.mahonkin.tim.extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.LoggingTest;

public static class LoggingTest
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.AddUnifiedLogger();
        })
        .Build();

        ILogger logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(LoggingTest));
        logger.LogTrace($"{LogLevel.Trace}/{LogLevel.Trace.LogType()}");
        logger.LogDebug($"{LogLevel.Debug}/{LogLevel.Debug.LogType()}");
        logger.LogInformation($"{LogLevel.Information}/{LogLevel.Information.LogType()}");
        logger.LogWarning($"{LogLevel.Warning}/{LogLevel.Warning.LogType()}");
        logger.LogError($"{LogLevel.Error}/{LogLevel.Error.LogType()}");
        logger.LogCritical($"{LogLevel.Critical}/{LogLevel.Critical.LogType()}");
    }
}