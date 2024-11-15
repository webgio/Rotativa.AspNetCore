using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.DemoApp.Models;
using System.Diagnostics;
using Rotativa.AspNetCore.Options;
using System.Data.Common;

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

        public IActionResult EphemerideWithAction()
        {
            return new ActionAsPdf("Ephemeride", new { d=DateTime.Now , day= DateTime.Now.DayOfWeek.ToString() } );
        }

        public IActionResult Ephemeride( DateTime d, string day)
        {
            Ephemeride ephemeride = new Ephemeride();
            ephemeride.DateOfTheDay = d;
            ephemeride.TheDayToday = day;

            ViewBag.testViewBag = new List<string> { "val 1", " val 2" };

            return View("EphemerideDemo", ephemeride);
        }

        public IActionResult ContactImage()
        {
            ViewData["Message"] = "Your contact page image.";

            // Example on how to set custom data.
            // For demo purposes we changed the name of the view, and specified that it isn't a partial view.
            // IsPartialView is false by default. We add some additional ViewData.
            // Using custom options 'Format' and 'Quality' as a demo.
            // See AsImageResultBase for more options.
            return new ViewAsImage("ContactDemo", isPartialView: false, viewData: ViewData, setBaseUrl: true)
            {
                Format = ImageFormat.png,
                Quality = 90
            };
        }

        public IActionResult PrivacyImage()
        {
            return new ViewAsImage();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}