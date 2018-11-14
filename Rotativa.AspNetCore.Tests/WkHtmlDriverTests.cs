using System.Reflection;
using System.Runtime.InteropServices;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Xunit;
using Path = System.IO.Path;

namespace Rotativa.AspNetCore.Tests
{
    public class WkHtmlDriverTests
    {
        private readonly string _wkHtmlExecutingPath;
        
        public WkHtmlDriverTests()
        {
            var windowsPath = Path.Combine(Assembly.GetExecutingAssembly().Location, "wkhtmltopdf.exe");
            var linuxPath = "/usr/local/bin/wkhtmltopdf";
        
            var isOsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            _wkHtmlExecutingPath = isOsWindows ? windowsPath : linuxPath;
        }

        [Fact]
        public void TestRunWkHtmlToPdf()
        {
            var dataForPdf = "test";
            var converter = new WkHtmlToPdfDriver(_wkHtmlExecutingPath);
            var pdfDoc = converter.ConvertHtml(string.Empty, dataForPdf);
            Assert.NotNull(pdfDoc);

            var pdfReader = new PdfReader(pdfDoc);
            Assert.Equal(1, pdfReader.NumberOfPages);

            var text = PdfTextExtractor.GetTextFromPage(pdfReader, 1, new SimpleTextExtractionStrategy());
            Assert.Equal(dataForPdf, text.Replace(" ", ""));
        }
    }
}