# UnifiedLogger

Implementation of an ILoggerProvider that writes log messages to Apple's Unified Logging system on iOS and Mac platforms.

## Getting started

### Prerequisites

- net7.0, net8.0, or nt9.0 SDK
- maui-maccatalyst and maui-ios workloads
- Net7, Net8, or Net9 MAUI project targeting iOS or MacCatalyst or similar ASP.Net Core web application

### Installation

- Download the latest NuPkg from [here.](https://github.com/timgreynolds/UnifiedLogging/releases)
- Use your preferred method to add the package to your project.
  Here's how I do it:
  - Create a `packages` directory and store the downloaded NuPkg files there.
  - Use the dotnet CLI to add this directory as a NuGet source.
    `dotnet nuget add source --name Local [path to the packages directory]`
  - Add a package reference in the project's CSPROJ.
    `<PackageReference Include="Mahonkin.UnifiedLogging" Version="1.0.*" />`
  - Run the command `dotnet restore` in the solution directory.

## Usage

- Add the UnifiedLoggerProvider to the dependency-injection container.
  In the class that creates the application builder and instantiates the app it should look something like this:
  ```*.cs
  var builder = AppBuilder.CreateBuilder();
  ...code removed for brevity...
  builder.Logging.AddUnifiedLogger();
  ```
- In any class that requires log capability add either an `ILoggerFactory` or `ILogger<TType>` object as a parameter in the class constructor. The dependency-injection container will ensure the correct object is passed.

  ```*.cs
  public class MyController : Controller
  {
    private ILogger _logger;

    public MyController(ILoggerFactory factory)
    {
      _logger = factory.CreateLogger<MyController>();
    }
  }
  ```

  or

  ```*.cs
  public class MyController : Controller
  {
    private ILogger _logger;

    public MyController(ILogger<MyController> logger)
    {
      _logger = logger;
    }
  }
  ```

* Use any of the `ILogger` logging methods to write messages to the log system.
  ```*.cs
  try
  {
    ...code removed for brevity...
  }
  catch(Exception ex)
  {
    _logger.LogCritical(ex.Message);
  }
  ```
* Use the Apple Console application to view the log messages.

## Configuration

Configuration, in general, is as with any other logging provider. The `Logging` section of `appsettings.json` provides options for logging. Individual providers can supply their own subsection within the `Logging` section. The section key for this package is `UnifiedLogging`.

```*.cs
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    },
    "UnifiedLogging": {
      "Subsystem: "MyAppName",
      "LogLevel": {
        "Default': "Warning",
        "Microsoft.Maui": "Debug"
      }
    }
  }
}
```

The `UnifiedLoggerProvider` configuration contains an optional `Subsystem` property. This is because the native Objective-C logging object requires two levels of categorization. Apple calls these `subsystem` and `category`. This package uses the term `category` more like the Microsoft interpretation and treats `subsytem` as an application or assembly categorization. `Subsystem` can be set, as above, in the settings file or it can be configured at compile-time when the provider is added to the dependency-injection container.

```*.cs
builder.logging.AddUnifiedLogging(options => {
  options.Subsystem = NSBundle.MainBundle.BundleIdentifier;
});
```

Passing the value in the `AddUnifiedLogging()` method will override the value in the settings file and thus removes the option for run-time configuration of `Subsystem`. Note the use of using BundleIdentifier in the  above example for a MAUI app. If not set in the file or overridden in `AddUnifiedLogging()` it will default to the string `UnifiedLogging`.

### Special Notes for MAUI apps

As of this time (early 2024) `MauiAppBuilder()` does not support automatically loading configuration from an appsettings file. This makes perfect sense because providing the user configuration options via a file on the device makes little sense in a phone idiom. The app developer would need to provide a mechanism to either edit this file or load the configuration from the file into custom platform-specific settings.

However, while developing and debugging an application it may be advantageous to the devloper to have this option available. Additionally, it allows the opportunity for the developer to take advantage of the methods mentioned; provide a mechanism to edit the configuration file or load the configuration into custom settings as well as supply a static configuration at install time.

1. Create an `appsettings.json` file in the MAUI project and place it in the `Resources\Raw` folder. (The file is not required to be called `appsettings.json`. Replace `appsettings.json` with any appropriate name.)
2. Place your logging configuration in the file. It is important that the root `Logging` element be included.

```*.cs
"Logging": {
  "LogLevel": {
    "Default": "Warning"
  },
  "UnifiedLogging": {
    "Subsystem": "MyApp",
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.Maui": "Debug"
    }
  }
}
```

3. On application install, or better yet every time the application starts, check for existence of this file in the device's `AppData` directory, and copy it there if not. This could be done in the `AppDelegate` class prior to the call that creates the `MauiApp`.
4. Add the `appsettings.json` file from the device's `AppData` directory as a JSON File Source to the application builder configuration sources list. I combine steps 3 and 4 during application building.

```*.cs
{
  MauiAppBuilder builder = MauiApp.CreateBuilder();
  builder.Configuration.AddJsonFile(GetConfig());
}

private string GetConfig()
{
  string appDataDir = Microsoft.Maui.Storage.FileSystem.Current.AppDataDirectory;
  string appSettingsFile = Path.Combine(appDataDir, "appsettings.json");
  if(File.Exists(appSettingsFile) == false)
  {
    using FileStream writeStream = File.Create(appSettingsFile);
    Microsoft.Maui.Storage.Filesystem.Current.OpenPackageFileAsync("appsettings.json").CopyTo(writeStream);
  }
  return appsettingsFile;
};
```
5. Get the `Logging` section of the builder's configuration and add it to the logging provider configuration.

```*.cs
{
  MauiAppBuilder builder = MauiApp.CreateBuilder();
  builder.Configuration.AddJsonFile(GetConfig());
  builder.Logging
    .ClearProviders()
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddUnifiedLogging();
}
```

## Additional documentation

- [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0)
- [Implement a custom Logging Provider](https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider)
- [Generating Log Messages in Your Code (Apple)](https://developer.apple.com/documentation/os/logging/generating_log_messages_from_your_code/)

## Feedback

[Github Repository Issues](https://github.com/timgreynolds/UnifiedLogging/issues)
