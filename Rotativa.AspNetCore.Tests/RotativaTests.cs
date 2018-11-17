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
    public class RotativaTests: RotativaBaseTest
    {
        [Fact(DisplayName ="should return the demo home page")]
        public void Is_the_site_reachable()
        {
            Assert.Equal("Home Page - Rotativa.AspNetCore.Demo", selenium.Title);
        }

        //[Fact]
        //public void Can_print_the_test_image()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test Image"));
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
        //public void Can_print_the_test_image_png()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test Image Png"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    using (var wc = new WebClient())
        //    {
        //        var imageResult = wc.DownloadData(new Uri(pdfHref));
        //        var image = Image.FromStream(new MemoryStream(imageResult));
        //        image.Should().Not.Be.Null();
        //        image.RawFormat.Should().Be.EqualTo(ImageFormat.Png);
        //    }
        //}

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
        //public void Can_print_the_pdf_from_a_view_with_non_ascii_chars()
        //{

        //    var testLink = selenium.FindElement(By.LinkText("Test View"));
        //    var pdfHref = testLink.GetAttribute("href");
        //    using (var wc = new WebClient())
        //    {
        //        var pdfResult = wc.DownloadData(new Uri(pdfHref));
        //        var pdfTester = new PdfTester();
        //        pdfTester.LoadPdf(pdfResult);
        //        pdfTester.PdfIsValid.Should().Be.True();
        //        pdfTester.PdfContains("àéù").Should().Be.True();
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
