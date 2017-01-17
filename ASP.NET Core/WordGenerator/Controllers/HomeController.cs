using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace wordGenerator.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            string LettersDigits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string RandWord = "";
            if(HttpContext.Session.GetInt32("Count") == null)
            {
                HttpContext.Session.SetInt32("Count", 0);
                ViewBag.Count = 0;
                ViewBag.Word = "";
            }
            else
            {
                var rand = new Random();
                for(int i = 0; i < 14; i++)
                    RandWord += LettersDigits[rand.Next(36)];
                int count = HttpContext.Session?.GetInt32("Count") ?? 0;
                HttpContext.Session.SetInt32("Count", ++count);
                ViewBag.Count = count;
                ViewBag.Word = RandWord;
            }
            return View();
        }

        [HttpGet("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
