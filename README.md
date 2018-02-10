# Rotativa.AspNetCore
Rotativa for Asp.Net Core 

## Development version
This is the firt version of Rotativa for Asp.Net Core.

Install with nuget.org:

https://www.nuget.org/packages/Rotativa.AspNetCore


Please give feedback!

## Needs configuration
Basic configuration done in Startup.cs:

```csharp
RotativaConfiguration.Setup(env);
```

Make sure you have a folder with the wkhtmltopdf.exe file accessible by the process running the web app. By default it searches in a folder named "Rotativa" in the root of the web app. If you need to change that use the optional parameter to the Setup call `RotativaConfiguration.Setup(env, "path/relative/to/root")`
