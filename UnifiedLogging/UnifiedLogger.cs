using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.UnifiedLogging;

public class UnifiedLogger : IDisposable, ILogger
{
    [DllImport("libSystem.dylib", EntryPoint = "os_log_create")]
    private static extern IntPtr os_log_create(string subsystem, string category);

    public UnifiedLogger(string category)
    {
        os_log_create(Assembly.GetCallingAssembly().GetName().FullName, category);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Console.WriteLine($"{logLevel} {eventId} {state} {exception?.Message} {formatter(state, exception)}");
    }

    public void Dispose()
    {
        Console.WriteLine("Diposing");
    }
}
