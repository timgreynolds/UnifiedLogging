using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.Logging.UnifiedLogging.Tests.Maui;

public partial class MainPage : ContentPage
{
	private ILogger _logger;

	public MainPage(ILoggerFactory factory)
	{
		_logger = factory.CreateLogger<MainPage>();
		InitializeComponent();
		foreach (string level in Enum.GetNames<LogLevel>())
		{
			_logger.LogDebug($"Adding {level} to the Picker list.");
			LevelPicker.Items.Add(level);
		}
		LevelPicker.SelectedIndex = (int)LogLevel.Debug;
	}

	private void OnCounterClicked(object? sender, EventArgs e)
	{
		if (Enum.TryParse<LogLevel>(LevelPicker.SelectedItem.ToString(), out LogLevel result))
		{
			_logger.Log(result, $"{LevelPicker.SelectedItem.ToString()} logged.");
		}
		else _logger.LogWarning($"Could not parse {LevelPicker.SelectedItem.ToString()} as a LogLevel.");
	}
}
