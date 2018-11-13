using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.Demo.Models;

namespace Rotativa.AspNetCore.Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            var model = new TestModel { Name = "Giorgio" };
            return new ViewAsPdf(model, ViewData);
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
