using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using quotingDojo.Models;
using quotingDojo.Factory;

namespace quotingDojo.Controllers
{
    public class QuotesController : Controller
    {
        private readonly QuoteFactory quoteFactory;
        public QuotesController()
        {
            quoteFactory = new QuoteFactory();
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.errors = TempData?["Errors"] ?? "";
            return View();
        }

        [HttpPost("quotes")]
        public IActionResult process(Quotes NewQuote)
        {
            TryValidateModel(NewQuote);
            if(ModelState.IsValid)
            {
                quoteFactory.Add(NewQuote);
                return RedirectToAction("quotes");
            }
            var errors = new List<string>();
            foreach(var error in ModelState.Values)
            {
                if(error.Errors.Count > 0)
                    errors.Add($"{error.Errors[0].ErrorMessage}");
            }
            TempData["Errors"] = errors;
            return RedirectToAction("index");
        }

        [HttpGet("quotes")]
        public IActionResult quotes()
        {
            ViewBag.Quotes = quoteFactory.FindAll();
            return View();
        }

        [HttpGet("quotes/likes/{id}")]
        public IActionResult updateLikes(int id)
        {
            int likes;
            var quote = quoteFactory.FindByID(id);
            likes = quote.Likes;
            likes += 1;
            quoteFactory.UpdateByID(id, likes);
            return RedirectToAction("quotes");
        }

        [HttpGet("quotes/delete/{id}")]
        public IActionResult delete(int id)
        {
            quoteFactory.DelByID(id);
            return RedirectToAction("quotes");
        }
    }
}
