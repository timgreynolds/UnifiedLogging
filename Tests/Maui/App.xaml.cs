using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.Logging.UnifiedLogging.Tests.Maui;

public partial class App : Application
{
	private ILoggerFactory _loggerFactory;

	public App(ILoggerFactory factory)
	{
		InitializeComponent();
		_loggerFactory = factory;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage(_loggerFactory));
	}
}
