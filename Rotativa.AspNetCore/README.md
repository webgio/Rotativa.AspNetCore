# Rotativa.AspNetCore

Rotativa for Asp.Net Core.

Docs are in the making. Should work almost exactly as Rotativa https://github.com/webgio/Rotativa

## Development version
This is the first version of Rotativa for Asp.Net Core.

Install with nuget.org:

https://www.nuget.org/packages/Rotativa.AspNetCore


Please give feedback!

## Needs configuration
Basic configuration done in Program.cs (.net 6, 7 or 8):

```csharp
app.UseRotativa();
```
or, if using .Net Core 3.1 and .Net 5:

```csharp
app.UseRotativa(env);
```

Make sure you have a folder with the wkhtmltopdf.exe file accessible by the process running the web app. By default it searches in a folder named "Rotativa" in the root of the web app. If you need to change that use the optional parameter to the Setup call `RotativaConfiguration.Setup(env, "path/relative/to/root")`

Place wkhtmltoimage.exe alongside wkhtmltopdf.exe in case you need to create images.

## Issues and Pull Request
Contribution is welcomed. If you would like to provide a PR please add some testing.


## rotativa.io

[rotativa.io](https://rotativa.io) is an API (SaaS) version of Rotativa, hosted on Azure. Works with just a HTTP call, no need to host the process on your server. You can register for free.
