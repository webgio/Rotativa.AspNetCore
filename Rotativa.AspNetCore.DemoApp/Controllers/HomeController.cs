using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.DemoApp.Models;
using System.Diagnostics;

namespace Rotativa.AspNetCore.DemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return new ViewAsPdf(viewData: ViewData);
        }

        public IActionResult Privacy()
        {
            return new ViewAsPdf();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}