using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Demo.Controllers
{
    public class MainController : Controller
    {
        List<Cat> cats = new List<Cat> {
                new Cat { Name="Kitty", Age=3},
                new Cat { Name="Garfield", Age=5},
                new Cat()
            };

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Cats = cats;
            return View();
        }

        [HttpPost("submit")]
        public IActionResult Submit(string name)
        {
            ViewBag.Name = name;
            ViewBag.Cats = cats;
            return View("index");
            // return RedirectToAction("Index");
        }
    }
    public class Cat
    {
        public string Name{ get; set; } = "Kitten";
        public int Age{ get; set; } = 1;
    }
}
