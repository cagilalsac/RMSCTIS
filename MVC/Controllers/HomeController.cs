using Microsoft.AspNetCore.Mvc;
using MVC.Controllers.Bases;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : MvcControllerBase // instead of inheriting from the Controller, we inherit from MvcControllerBase to set the culture
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /*
        IActionResult
        |
        (implements IActionResult) ActionResult 
        |
        (inherits from ActionResult) ViewResult - ContentResult - RedirectResults - HttpStatusCodeResults
        */
        // Way 1:
        //public ViewResult Index()
        // Way 2:
        //public ActionResult Index()
        // Way 3:
        public IActionResult Index()
        {
            // Way 1:
            //return new ViewResult();
            // Way 2:
            return View(); // returns an object of type ViewResult
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}