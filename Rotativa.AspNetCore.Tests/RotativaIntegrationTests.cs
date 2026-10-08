using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rotativa.AspNetCore.Tests
{

    [Trait("Rotativa.AspNetCore", "accessing the demo site home page")]
    public class RotativaIntegrationTests : IDisposable
    {
        ChromeDriver selenium;
        StringBuilder verificationErrors;

        // The demo site to test against. Override with ROTATIVA_DEMO_URL (e.g. "http://localhost:5000" on Linux/CI).
        public static IEnumerable<object[]> DemoSites => new[]
        {
            new object[] { Environment.GetEnvironmentVariable("ROTATIVA_DEMO_URL") ?? "https://localhost:56246", "Asp.net 10" }
        };

        // Run Chrome headless when ROTATIVA_TESTS_HEADLESS is set, or on CI servers (which set CI=true).
        static bool Headless =>
            IsTrue(Environment.GetEnvironmentVariable("ROTATIVA_TESTS_HEADLESS")) || IsTrue(Environment.GetEnvironmentVariable("CI"));

        static bool IsTrue(string? value) => value == "1" || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);

        public  RotativaIntegrationTests()
        {
            var options = new ChromeOptions();
            if (Headless)
            {
                // --no-sandbox and --disable-dev-shm-usage let Chrome start inside containers.
                options.AddArguments("--headless=new", "--no-sandbox", "--disable-dev-shm-usage");
            }
            // Optional path to a Chrome/Chromium binary, for machines where Selenium can't find or download one.
            var chromeBinary = Environment.GetEnvironmentVariable("ROTATIVA_CHROME_BINARY");
            if (!string.IsNullOrEmpty(chromeBinary)) options.BinaryLocation = chromeBinary;

            selenium = new ChromeDriver(options);
            //selenium = new InternetExplorerDriver();
            selenium.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 10);
            verificationErrors = new StringBuilder();
        }

        public void Dispose()
        {
            if (selenium != null) selenium.Quit();
        }

        [Theory(DisplayName = "should return the demo home page")]
        //[InlineData("http://localhost:64310", "Asp.net core 2.0")]
        //[InlineData("https://localhost:44375", "Asp.net core 3.1")]
        //[InlineData("https://localhost:7059", "Asp.net 6")]
        [MemberData(nameof(DemoSites))]
        public void Is_the_site_reachable(string url, string site)
        {
            selenium.Navigate().GoToUrl(url);

            Assert.Equal("Home Page - Rotativa.AspNetCore.Demo", selenium.Title);
        }

        [Theory(DisplayName = "can get the PDF from the contact link")]
        //[InlineData("http://localhost:64310", "Asp.net core 2.0")]
        //[InlineData("https://localhost:44375", "Asp.net core 3.1")]
        //[InlineData("https://localhost:7059", "Asp.net 6")]
        [MemberData(nameof(DemoSites))]
        public async Task Contact_PDF_ViewData(string url, string site)
        {
            selenium.Navigate().GoToUrl(url);

            var testLink = selenium.FindElement(By.LinkText("Contact"));
            var pdfHref = testLink.GetAttribute("href");

            using (var wc = new HttpClient())
            {
                var pdfResult = await wc.GetAsync(new Uri(pdfHref));
                var pdfTester = new PdfTester();
                pdfTester.LoadPdf(await pdfResult.Content.ReadAsByteArrayAsync());
                Assert.True(pdfTester.PdfIsValid);

                // This should be present, as it's set in the viewdata.
                Assert.True(pdfTester.PdfContains("Your contact page."));
            }
        }

        [Theory(DisplayName = "can get a PDF with special characters")]
        //[InlineData("http://localhost:64310", "Asp.net core 2.0")]
        //[InlineData("https://localhost:44375", "Asp.net core 3.1")]
        //[InlineData("https://localhost:7059", "Asp.net 6")]
        [MemberData(nameof(DemoSites))]
        public async Task Contact_PDF_SpecialCharacters(string url, string site)
        {
            selenium.Navigate().GoToUrl(url);
            var testLink = selenium.FindElement(By.LinkText("Contact"));
            var pdfHref = testLink.GetAttribute("href");

            using (var wc = new HttpClient())
            {
                var pdfResult = await wc.GetAsync(new Uri(pdfHref));
                var pdfTester = new PdfTester();
                pdfTester.LoadPdf(await pdfResult.Content.ReadAsByteArrayAsync());

                Assert.True(pdfTester.PdfIsValid);
                Assert.True(pdfTester.PdfContains("àéù"));
            }
        }

        [Theory(DisplayName = "can get the png from the contact image link")]
        //[InlineData("http://localhost:64310", "Asp.net core 2.0")]
        //[InlineData("https://localhost:44375", "Asp.net core 3.1")]
        //[InlineData("https://localhost:7059", "Asp.net 6")]
        [MemberData(nameof(DemoSites))]
        public async Task Can_create_png_image(string url, string site)
        {
            selenium.Navigate().GoToUrl(url);

            var testLink = selenium.FindElement(By.LinkText("Contact Image"));
            var pdfHref = testLink.GetAttribute("href");

            using (var wc = new HttpClient())
            {
                var imageResult = await wc.GetAsync(new Uri(pdfHref));
                var image = await imageResult.Content.ReadAsByteArrayAsync();

                // PNG file signature; checked directly because System.Drawing is Windows-only.
                Assert.True(image.AsSpan().StartsWith(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }));
            }
        }

        [Theory(DisplayName = "can get the jpg from the privacy image link")]
        //[InlineData("http://localhost:64310", "Asp.net core 2.0")]
        //[InlineData("https://localhost:44375", "Asp.net core 3.1")]
        //[InlineData("https://localhost:7059", "Asp.net 6")]
        [MemberData(nameof(DemoSites))]
        public async Task Can_create_jpg_image(string url, string site)
        {
            selenium.Navigate().GoToUrl(url);

            var testLink = selenium.FindElement(By.LinkText("Privacy Image"));
            var pdfHref = testLink.GetAttribute("href");

            using (var wc = new HttpClient())
            {
                var imageResult = await wc.GetAsync(new Uri(pdfHref));
                var image = await imageResult.Content.ReadAsByteArrayAsync();

                // JPEG SOI marker; checked directly because System.Drawing is Windows-only.
                Assert.True(image.AsSpan().StartsWith(new byte[] { 0xFF, 0xD8, 0xFF }));
            }
        }

        //[Fact]
        //public void Can_print_the_authorized_pdf()
        //{
        //    //// Log Off if required...
        //    var logoffLink = selenium.FindElements(By.LinkText("Log Off"));
        //    if (logoffLink.Any())
        //        logoffLink.First().Click();

        //    var testLink = selenium.FindElement(By.LinkText("Logged In Test"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var loginLink = selenium.FindElement(By.ClassName("logon"));
        //    loginLink.Click();

        //    var username = selenium.FindElement(By.Id("UserName"));
        //    username.SendKeys("admin");
        //    var password = selenium.FindElement(By.Id("Password"));
        //    password.Clear();
        //    password.SendKeys("admin");
        //    password.Submit();
        //    var manage = selenium.Manage();
        //    var cookies = manage.Cookies.AllCookies;
        //    using (var wc = new WebClient())
        //    {
        //        foreach (var cookie in cookies)
        //        {
        //            var cookieText = cookie.Name + "=" + cookie.Value;
        //            wc.Headers.Add(HttpRequestHeader.Cookie, cookieText);
        //        }
        //        var pdfResult = wc.DownloadData(new Uri(pdfHref));
        //        var pdfTester = new PdfTester();
        //        pdfTester.LoadPdf(pdfResult);
        //        pdfTester.PdfIsValid.Should().Be.True();
        //        pdfTester.PdfContains("My MVC Application").Should().Be.True();
        //        pdfTester.PdfContains("admin").Should().Be.True();
        //    }
        //}

        //[Fact]
        //public void Can_print_the_authorized_image()
        //{
        //    //// Log Off if required...
        //    var logoffLink = selenium.FindElements(By.LinkText("Log Off"));
        //    if (logoffLink.Any())
        //        logoffLink.First().Click();

        //    var testLink = selenium.FindElement(By.LinkText("Logged In Test Image"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    testLink.Click();

        //    var username = selenium.FindElement(By.Id("UserName"));
        //    username.SendKeys("admin");
        //    var password = selenium.FindElement(By.Id("Password"));
        //    password.Clear();
        //    password.SendKeys("admin");
        //    password.Submit();
        //    var manage = selenium.Manage();
        //    var cookies = manage.Cookies.AllCookies;
        //    using (var wc = new WebClient())
        //    {
        //        foreach (var cookie in cookies)
        //        {
        //            var cookieText = cookie.Name + "=" + cookie.Value;
        //            wc.Headers.Add(HttpRequestHeader.Cookie, cookieText);
        //        }
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Jpeg);
        //    }
        //}

        //[Fact]
        //public void Can_print_the_pdf_from_a_view()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test View"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    using (var wc = new WebClient())
        //    {
        //        var pdfResult = wc.DownloadData(new Uri(pdfHref));
        //        var pdfTester = new PdfTester();
        //        pdfTester.LoadPdf(pdfResult);
        //        pdfTester.PdfIsValid.Should().Be.True();
        //        pdfTester.PdfContains("My MVC Application").Should().Be.True();
        //    }
        //}

        //[Fact]
        //public void Can_print_the_image_from_a_view()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test View Image"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    using (var wc = new WebClient())
        //    {
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Jpeg);
        //    }
        //}

        //[Fact]
        //public void Can_print_the_image_from_a_view_with_non_ascii_chars()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test View Image"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    using (var wc = new WebClient())
        //    {
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Jpeg);
        //    }
        //}

        //[Fact]
        //public void Can_print_the_pdf_from_a_view_with_a_model()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test ViewAsPdf with a model"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var title = "This is a test";
        //    using (var wc = new WebClient())
        //    {
        //        var pdfResult = wc.DownloadData(new Uri(pdfHref));
        //        var pdfTester = new PdfTester();
        //        pdfTester.LoadPdf(pdfResult);
        //        pdfTester.PdfIsValid.Should().Be.True();
        //        pdfTester.PdfContains(title).Should().Be.True();
        //    }
        //}

        //[Fact]
        //public void Can_print_the_image_from_a_view_with_a_model()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test ViewAsImage with a model"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var title = "This is a test";
        //    using (var wc = new WebClient())
        //    {
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Jpeg);
        //    }
        //}

        //[Fact]
        //public void Can_print_the_pdf_from_a_partial_view_with_a_model()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test PartialViewAsPdf with a model"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var content = "This is a test with a partial view";
        //    using (var wc = new WebClient())
        //    {
        //        var pdfResult = wc.DownloadData(new Uri(pdfHref));
        //        var pdfTester = new PdfTester();
        //        pdfTester.LoadPdf(pdfResult);
        //        pdfTester.PdfIsValid.Should().Be.True();
        //        pdfTester.PdfContains(content).Should().Be.True();
        //    }
        //}

        //[Fact]
        //public void Can_print_the_image_from_a_partial_view_with_a_model()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test PartialViewAsImage with a model"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var content = "This is a test with a partial view";
        //    using (var wc = new WebClient())
        //    {
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Jpeg);
        //    }
        //}

        //[Fact]
        //public void Can_print_pdf_from_page_with_content_from_ajax_request()
        //{
        //    var testLink = selenium.FindElement(By.LinkText("Ajax Test"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var content = "Hi there, this is content from a Ajax call.";
        //    using (var wc = new WebClient())
        //    {
        //        var pdfResult = wc.DownloadData(new Uri(pdfHref));
        //        var pdfTester = new PdfTester();
        //        pdfTester.LoadPdf(pdfResult);
        //        pdfTester.PdfIsValid.Should().Be.True();
        //        pdfTester.PdfContains(content).Should().Be.True();
        //    }
        //}

        //[Fact]
        //public void Can_print_image_from_page_with_content_from_ajax_request()
        //{
        //    var testLink = selenium.FindElement(By.LinkText("Ajax Image Test"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var content = "Hi there, this is content from a Ajax call.";
        //    using (var wc = new WebClient())
        //    {
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Jpeg);
        //    }
        //}

        //[Fact]
        //public void Can_print_pdf_from_page_with_external_css_file()
        //{
        //    var testLink = selenium.FindElement(By.LinkText("External CSS Test"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var content = "Hi guys, this content shows up thanks to css file.";
        //    using (var wc = new WebClient())
        //    {
        //        var pdfResult = wc.DownloadData(new Uri(pdfHref));
        //        var pdfTester = new PdfTester();
        //        pdfTester.LoadPdf(pdfResult);
        //        pdfTester.PdfIsValid.Should().Be.True();
        //        pdfTester.PdfContains(content).Should().Be.True();
        //    }
        //}

        //[Fact]
        //public void Can_print_image_from_page_with_external_css_file()
        //{
        //    var testLink = selenium.FindElement(By.LinkText("External CSS Test Image"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    var content = "Hi guys, this content shows up thanks to css file.";
        //    using (var wc = new WebClient())
        //    {
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Jpeg);
        //    }
        //}
    }
}
