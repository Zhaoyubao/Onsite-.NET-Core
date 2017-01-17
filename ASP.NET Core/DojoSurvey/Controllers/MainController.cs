using System;
using Microsoft.AspNetCore.Mvc;

namespace DojoSurvey.Controllers
{
    public class MainController : Controller
    {
        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpPost("result")]
        public IActionResult Result(string name, string location, string language, string comment)
        {
            if(String.IsNullOrWhiteSpace(name))
                return RedirectToAction("index");
            ViewBag.Name = name;
            ViewBag.Location = location;
            ViewBag.Language = language;
            ViewBag.Comment = comment;
            return View();
        }
    }
}
