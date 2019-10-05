# Rotativa.AspNetCore

Rotativa for Asp.Net Core.

Docs are in the making. Should work almost exactly as Rotativa https://github.com/webgio/Rotativa

## Development version
This is the first version of Rotativa for Asp.Net Core.

Install with nuget.org:

https://www.nuget.org/packages/Rotativa.AspNetCore


Please give feedback!

## Needs configuration
Basic configuration done in Startup.cs:

```csharp
RotativaConfiguration.Setup(env);
```

Make sure you have a folder with the wkhtmltopdf.exe file accessible by the process running the web app. By default it searches in a folder named "Rotativa" in the root of the web app. If you need to change that use the optional parameter to the Setup call `RotativaConfiguration.Setup(env, "path/relative/to/root")` or if using Asp.net core 3.0: `RotativaConfiguration.Setup("<root path here>", "path/relative/to/root")`

## Issues and Pull Request
Contribution is welcomed. If you would like to provide a PR please add some testing.
