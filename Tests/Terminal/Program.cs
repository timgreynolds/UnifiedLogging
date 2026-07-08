using com.mahonkin.tim.extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.Logging.UnifiedLogging.Tests.Terminal;

internal class Program
{
  private static void Main(string[] args)
  {
    var builder = Host.CreateApplicationBuilder(args);
    builder
      .Logging
        .ClearProviders()
        .AddUnifiedLogger()
        .AddDebug();

    builder.Services.AddHostedService<Worker>();

    var host = builder.Build();
    host.Run();
  }
}