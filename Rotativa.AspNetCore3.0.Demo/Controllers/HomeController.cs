using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore3._0.Demo.Models;

namespace Rotativa.AspNetCore3._0.Demo.Controllers
{
    public class TestModel
    {
        public string Name { get; set; }
    }

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

            return new ViewAsPdf("~/Views/Home/Contact.cshtml", viewData: ViewData);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
