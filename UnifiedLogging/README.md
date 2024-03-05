# UnifiedLogger

Implementation of an ILoggerProvider to write log messages to Apple's Unified Logging system on iOS and Mac platforms.

## Getting started

### Prerequisites

* net7.0 or net8.0 SDK
* maui-maccatalyst and maui-ios workloads
* Net7 or Net8 MAUI project targeting iOS or MacCatalyst
  or Net7/Net8 ASP.Net Core web application

### Installation

* Download the latest NuPkg from [here.](https://github.com/timgreynolds/UnifiedLogging/releases)
* Use your preferred method to add the package to your project.
  Here's how I do it:
  * Create a `packages` directory in your development environment and store the downloaded NuPkg files there.
  * Use the dotnet CLI to add this directory as a NuGet source. 
  `dotnet nuget add source --name Local [path to the packages directory]`
  * Add a package reference in the project's CSPROJ. 
  `<PackageReference Include="Mahonkin.UnifiedLogging" Version="1.0.*" />`
  * Run the command `dotnet restore` in the solution directory.

## Usage

* Add the UnifiedLoggerProvider to the dependency-injection container.
  In the class that creates the application builder and instantiates the app it should look something like this:
```
    var builder = AppBuilder.CreateBuilder();
    ...code removed for brevity...
    builder.Logging.AddUnifiedLogger();
```
* 

## Configuration

## Additional documentation

* [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0)
* [Implement a custom Logging Provider](https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider)
* [Generating Log Messages in Your Code (Apple)](https://developer.apple.com/documentation/os/logging/generating_log_messages_from_your_code/)

## Feedback

[Github Repository Issues](https://github.com/timgreynolds/UnifiedLogging/issues)