using System.Collections.Generic;
using com.mahonkin.tim.extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace com.mahonkin.tim.Logging.UnifiedLogging.Tests.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
		{
			["Logging:LogLevel:Default"] = "Warning",
			["Logging:LogLevel:Microsoft.Maui.Hosting"] = "Debug",
			["Logging:Debug:LogLevel:Default"] = "Debug",
			["Logging:UnifiedLogging:LogLevel:Default"] = "Warning",
			["Logging:UnifiedLogging:LogLevel:com.mahonkin.tim"] = "Debug"
		});
		builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))
			.ClearProviders()
			.AddUnifiedLogger();

#if DEBUG
		builder.Logging.SetMinimumLevel(LogLevel.Debug).AddDebug();
#endif

		return builder.Build();
	}
}
