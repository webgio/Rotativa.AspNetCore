using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.Demo.Models;

namespace Rotativa.AspNetCore.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly WkHtmlToPdfDriver _pdfDriver;

        public HomeController(WkHtmlToPdfDriver pdfDriver)
        {
            _pdfDriver = pdfDriver;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            var model = new TestModel { Name = "Giorgio" };
            return new ViewAsPdf(model, _pdfDriver, ViewData);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
