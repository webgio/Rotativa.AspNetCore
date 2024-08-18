# Create PDFs and images with .NET

Use Rotativa to transform a Razor view into a PDF or image.
This package is compatible with .NET Core 3.1, .NET 5, .NET 6, .NET 7 and .NET 8.

## Install with nuget.org:

https://www.nuget.org/packages/Rotativa.AspNetCore


Please give feedback!

## Needs configuration
Basic configuration done in Program.cs (.NET 6 up to 8):

```csharp
app.UseRotativa();
```
or, if using .NET Core 3.1 and .NET 5:

```csharp
app.UseRotativa(env);
```

Make sure you have a folder with the wkhtmltopdf.exe file accessible by the process running the web app. By default it searches in a folder named "Rotativa" in the root of the web app. If you need to change that use the optional parameter to the Setup call `RotativaConfiguration.Setup(env, "path/relative/to/root")`

## Usage

This package should work almost exactly as Rotativa https://github.com/webgio/Rotativa.

Instead of returning a `View()` in your .NET controller, use `new ViewAsPdf()` to return a PDF or use `new ViewAsImage()` to return an image:

```csharp
public class InvoiceController : Controller
{
    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(ILogger<InvoiceController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Returns the Index view as HTML.
        return View();
    }

    public IActionResult Invoice()
    {
        // Returns the Invoice view as PDF.
        return new ViewAsPdf();
    }

    public IActionResult InvoiceImage()
    {
        // Returns the InvoiceImage view as PDF.
        return new ViewAsImage();
    }
}
```

You can specify the View that should be transformed into a PDF or image:

```csharp
return new ViewAsPdf("NameOfView");
```

Pass ViewData as an optional property:

```csharp
ViewData["Message"] = "Thank you for downloading this PDF.";
return new ViewAsPdf(viewData: ViewData);
```

We support partial views as well:

```csharp
return new ViewAsImage("MyPartialView", isPartialView: true);
```

By default Rotativa injects a base url in the head section of the HTML. This can be disabled:

```csharp
return new ViewAsImage(setBaseUrl: false);
```

The settings can be combined as well:

```csharp
ViewData["Message"] = "Thank you for downloading this PDF.";
return new ViewAsImage("MyPartialView", isPartialView: true, viewData: ViewData, setBaseUrl: false);
```

To change the way the PDF or image is generated, you can pass the settings as parameters:

```csharp
return new ViewAsImage()
{
    Format = ImageFormat.png,
    Quality = 90
};
```

By default the PDF or image is shown to the user in the browser, like HTML. If you want to force the document to be downloaded use the Content-Disposition property:

```csharp
return new ViewAsPdf()
{
    ContentDisposition = ContentDisposition.Attachment,
    FileName = "MyDocument.pdf"
};
```

Each property is documented in the object for easy reference.

Rotativa uses wkhtmltopdf/wkhtmltoimage behind the scenes. If you want to specify custom switches that are unsupported by Rotativa, you can pass them as well:

```csharp
return new ViewAsPdf()
{
    CustomSwitches = "--disable-smart-shrinking"
};
```

If you need to write the PDF to the server, you can call `BuildFile` and use the resulting byte array to save the file:

```csharp
var pdfFile = new ViewAsPdf().BuildFile(this.ControllerContext);
File.WriteAllBytes("wwwroot/output.pdf", pdfFile);
```

This is how you save the PDF file to the server before displaying it in the browser:

```csharp
public IActionResult Invoice()
{
    // Generate the PDF.
    var pdfFile = new ViewAsPdf();
    
    // Save to the server.
    File.WriteAllBytes("wwwroot/output.pdf", pdfFile.BuildFile(this.ControllerContext));

    // Show in the browser.
    return pdfFile;
}
```

## Issues and Pull Request
Contribution is welcomed. If you would like to provide a PR please add some testing.


## rotativa.io

[rotativa.io](https://rotativa.io) is an API (SaaS) version of Rotativa, hosted on Azure. Works with just a HTTP call, no need to host the process on your server. You can register for free.
