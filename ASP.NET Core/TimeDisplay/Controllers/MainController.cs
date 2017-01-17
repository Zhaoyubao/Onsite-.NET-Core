using System;
using Microsoft.AspNetCore.Mvc;

namespace TimeDisplay.Controllers
{
    public class MainController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Date = DateTime.Now.ToString("MMM dd, yyyy");
            ViewBag.Time = DateTime.Now.ToString("hh:mm tt");
            return View();
        }
    }
}
