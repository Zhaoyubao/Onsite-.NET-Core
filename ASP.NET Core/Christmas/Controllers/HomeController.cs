using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace christmas.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetObjectFromJson<ChristmasTree>("ourTree") == null )
            {
                var ourTree = new ChristmasTree();
                HttpContext.Session.SetObjectAsJson("ourTree", ourTree);  
            }
            ViewBag.ourTree = HttpContext.Session.GetObjectFromJson<ChristmasTree>("ourTree");
            return View();
        }
        [HttpPost]
        [Route("chop")]
        public IActionResult Chop()
        {
            ChristmasTree ourTree = HttpContext.Session.GetObjectFromJson<ChristmasTree>("ourTree");
            ourTree.chop();
            HttpContext.Session.SetObjectAsJson("ourTree", ourTree);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("decorate")]
        public IActionResult Decorate()
        {
            ChristmasTree ourTree = HttpContext.Session.GetObjectFromJson<ChristmasTree>("ourTree");
            ourTree.decorate();
            HttpContext.Session.SetObjectAsJson("ourTree", ourTree);
            return RedirectToAction("Index");
        }
    }
    public class ChristmasTree
    {
        public int health = 100;
        public int decorating = 0;
        public bool kill = false;
        public bool pretty = false;

        public void chop()
        {
            health -= new Random().Next(1,50);
            timber();
        }
        public void decorate()
        {
            decorating += new Random().Next(1,20);
            eggnog();
        }
        public bool timber()
        {
            if(health <= 0)
            {
                kill = true;
            }
            return kill;
        }
        public bool eggnog()
        {
            if(decorating >= 100)
            {
                pretty = true;
            }
            return pretty;
        }
    }
}
