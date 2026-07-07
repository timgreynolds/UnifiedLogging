using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.Logging.UnifiedLogging.Tests.Terminal;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {

        while (!token.IsCancellationRequested)
        {
            foreach (LogLevel level in Enum.GetValues<LogLevel>())
            {
                if (_logger.IsEnabled(level))
                {
                    _logger.Log(level, "Worker running at: {time}", DateTime.Now);
                }
            }
        }
    }
}
