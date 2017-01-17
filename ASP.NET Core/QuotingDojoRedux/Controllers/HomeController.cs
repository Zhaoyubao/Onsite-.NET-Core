using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using QuotingRedux.Models;
using QuotingRedux.Factory;

namespace QuotingRedux.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserFactory userFactory;
        private readonly QuoteFactory quoteFactory;
        public HomeController(UserFactory user, QuoteFactory quote) {
            userFactory = user;
            quoteFactory = quote;
        }
       
// ================================================================================

        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("quotes");
            ViewBag.RegErrors = TempData?["RegErrors"] ?? "";
            ViewBag.RegEmailErr = TempData["RegEmailErr"];
            ViewBag.LogPwErr = TempData["LogPwErr"];
            ViewBag.LogEmailErr = TempData["LogEmailErr"];
            return View();
        }
// ================================================================================

        [HttpPost("register")]
        public IActionResult register(User NewUser)
        {
            TryValidateModel(NewUser);
            if(ModelState.IsValid)
            {
                User user = userFactory.FindByEmail(NewUser.Email);
                if(user == null) 
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                    userFactory.Add(NewUser);
                    HttpContext.Session.SetInt32("UserID", userFactory.FindByEmail(NewUser.Email).Id);
                    return RedirectToAction("quotes");
                }
                TempData["RegEmailErr"] = "Email already exists!";
            }
            var errors = new List<string>();
            foreach(var error in ModelState.Values)
            {
                if(error.Errors.Count > 0)
                    errors.Add($"{error.Errors[0].ErrorMessage}");
            }
            TempData["RegErrors"] = errors;
            return RedirectToAction("index");
        }
// ================================================================================

        [HttpPost("login")]
        public IActionResult login(string email, string pw)
        {
            User user = userFactory.FindByEmail(email);
            if(user != null) 
            {
                var Hasher = new PasswordHasher<User>();
                if(Hasher.VerifyHashedPassword(user, user.Password, pw) != 0)
                {
                    HttpContext.Session.SetInt32("UserID", user.Id);
                    return RedirectToAction("quotes");
                }
                else TempData["LogPwErr"] = "Incorrect password!";
            }
            else TempData["LogEmailErr"] = "Email address does not exist!";
            return RedirectToAction("index");
        }
// ================================================================================
        
        [HttpGet("quotes")]
        public IActionResult quotes()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("index");
            ViewBag.User = userFactory.FindByID((int)HttpContext.Session.GetInt32("UserID"));
            ViewBag.Errors = TempData["Errors"];
            ViewBag.Quotes = quoteFactory.FindAll();
            return View();
        }
// ================================================================================

        [HttpPost("quotes")]
        public IActionResult addQuotes(Quote NewQuote)
        {
            int userID = (int)HttpContext.Session.GetInt32("UserID");
            TryValidateModel(NewQuote);
            if(ModelState.IsValid)
            {
                quoteFactory.Add(NewQuote, userID);
                return RedirectToAction("quotes");
            }
            foreach(var error in ModelState.Values)
            {
                if(error.Errors.Count > 0)
                    TempData["Errors"] = error.Errors[0].ErrorMessage;
            }
            return RedirectToAction("quotes");
        }
// ================================================================================
        
        [HttpGet("quotes/likes/{id}")]
        public IActionResult updateLikes(int id)
        {
            var quote = quoteFactory.FindByID(id);
            int likes = quote.Likes;
            quoteFactory.UpdateByID(id, ++likes);
            return RedirectToAction("quotes");
        }
// ================================================================================

        [HttpGet("quotes/delete/{id}")]
        public IActionResult delete(int id)
        {
            quoteFactory.DelByID(id);
            return RedirectToAction("quotes");
        }
// ================================================================================

        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }
    }
}
