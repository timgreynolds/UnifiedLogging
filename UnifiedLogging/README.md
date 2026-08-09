# UnifiedLogger

Implementation of an ILoggerProvider that writes log messages to Apple's Unified Logging system on iOS and Mac platforms.

## Getting started

### Prerequisites

- net9.0 SDK
- maui-maccatalyst and maui-ios workloads
- Net9 MAUI project targeting iOS or MacCatalyst or similar ASP.Net Core web application

### Installation

- Install the package from the GitHub NuGet repository, or
- Download the latest NuPkg from [here.](https://github.com/timgreynolds/UnifiedLogging/releases)
- Use your preferred method to add the package to your project.
  Here's how I do it:
  - Create a `packages` directory and store the downloaded NuPkg files there.
  - Use the dotnet CLI to add this directory as a NuGet source.
    `dotnet nuget add source --name Local [path to the packages directory]`
  - Add a package reference in the project's CSPROJ.
    `<PackageReference Include="Mahonkin.UnifiedLogging" />`
  - Run the command `dotnet restore` in the solution directory.

## Usage

- Add the UnifiedLoggerProvider to the dependency-injection container.
  In the class that creates the application builder and instantiates the app it should look something like this:
  ```*.cs
  var builder = AppBuilder.CreateBuilder();
  ...code removed for brevity...
  builder.Logging.AddUnifiedLogger();
  ```
- In any class that requires log capability add either an `ILoggerFactory` or `ILogger<T>` object as a parameter in the class constructor. The dependency-injection container will ensure the correct object is passed.

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

- Use any of the `ILogger` logging methods to write messages to the log system.

  ```*.cs
  try
  {
    ...code removed for brevity...
  }
  catch(Exception ex)
  {
    _logger.LogCritical("An exception occurred: {message}", ex.Message);
  }
  ```

  - Custom formatting strings `Priv` and `Mask` can be used to mimic Apple's [OSLogPrivacy](https://developer.apple.com/documentation/os/oslogprivacy) options. See [Known Issues](#known_issues).

    ```*.cs
    try
    {
      ...code removed for brevity...
    }
    catch(Exception ex)
    {
      _logger.LogError("An exception occurred, {message}, while retrieving account details for: {accountnum:Priv}", ex.Message, accountnum);
    }
    ```

- Use the Apple Console application to view the log messages.

## Configuration

Configuration, in general, is as with any other logging provider. The `Logging` section of `appsettings.json` provides options for logging. Individual providers supply their own subsection within the `Logging` section. The section key for this package is `UnifiedLogging`.

```appsettings.json
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

The `UnifiedLoggerProvider` configuration contains an optional `Subsystem` property. This is because the native Objective-C logging object requires two levels of categorization. Apple calls these `Subsystem` and `Category`. This package uses the term `Category` more like the Microsoft interpretation; it is the name of the type passed to the ILogger object. `Subsytem` is treated as an application or assembly-level categorization. Since it is a require parameter, `Subsystem` can be set, as in the example above, in the settings file. Alernatively, it can be configured at compile-time when the provider is added to the dependency-injection container.

```*.cs
builder.logging.AddUnifiedLogging(options => {
  options.Subsystem = NSBundle.MainBundle.BundleIdentifier;
});
```

Passing the value in the `AddUnifiedLogging()` method will override the value in the settings file and thus removes the option for run-time configuration of `Subsystem`. Note the use of using BundleIdentifier in the above example for a MAUI app. If not set in the file or overridden in `AddUnifiedLogging()` the value will default to the string `UnifiedLogging`.

### Special Notes for MAUI apps

Maui applications do not natively support `appsettings.json` as a configuration source. This makes perfect sense because providing the user configuration options via a file on the device makes little sense in a phone idiom. The app developer would need to provide a mechanism to either edit this file or load the configuration from the file into custom platform-specific settings.

However, while developing and debugging an application it may be advantageous to the devloper to have this option available. Additionally, it allows the opportunity for the developer to take advantage of the methods mentioned; provide a mechanism to edit the configuration file or load the configuration into custom settings as well as supply a static configuration at install time.

1. Create an `appsettings.json` file in the MAUI project and place it in the `Resources\Raw` folder. (The file is not required to be called `appsettings.json`. Replace `appsettings.json` with any appropriate name.)
2. Place your logging configuration in the file. It is important that the root `Logging` element be included.

```appsettings.json
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

```MauiProgram.cs
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

```MauiProgram.cs
{
  MauiAppBuilder builder = MauiApp.CreateBuilder();
  builder.Configuration.AddJsonFile(GetConfig());
  builder.Logging
    .ClearProviders()
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddUnifiedLogging();
}
```

<a id="known_issues" ></a>

## Known Issues

- `Priv` and `Mask` are **only** supported for the Unified Logging provider. Do not expect them to work with other output.
- Be careful using `Priv` and `Mask` with other logging providers that may use formatters for known types. For example, Enum types in Console and Debug only support a specific list of format strings and will throw an exception if anything else is provided.
  - If necessary call the type's `ToString()` on the variable in the call to `ILogger.Log<T>()`

  ```*.cs
  _logger.LogDebug("{level:Mask}: Retrieving account details for: {accountnum:Priv}", LogLevel.Debug.ToString(), accountnum);
  ```

## Additional documentation

- [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-9.0)
- [Implement a custom Logging Provider](https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider)
- [Generating Log Messages in Your Code (Apple)](https://developer.apple.com/documentation/os/logging/generating_log_messages_from_your_code/)

## Feedback

[Github Repository Issues](https://github.com/timgreynolds/UnifiedLogging/issues)
