using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuotingRedux.Models;

namespace QuotingRedux.Controllers
{
    public class HomeController : Controller
    {
        private MysqlContext db;
        public HomeController(MysqlContext context)
        {
            db = context;
        }
       
// ================================================================================

        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("quotes");
            ViewBag.LogPwErr = TempData["LogPwErr"];
            ViewBag.LogEmailErr = TempData["LogEmailErr"];
            return View();
        }
// ================================================================================

        [HttpPost("register")]
        public IActionResult register(UserViewModel model)
        {
            User user = db.Users.SingleOrDefault( u => u.Email == model.Email );
            if(ModelState.IsValid && user == null)
            {
                User NewUser = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, model.Password);
                db.Add(NewUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UserID", db.Users.SingleOrDefault( u => u.Email == model.Email ).UserId);
                return RedirectToAction("quotes");
            }
            else if(user != null)
                ViewBag.RegEmailErr = "Email already exists!";
            return View("index");
        }
// ================================================================================

        [HttpPost("login")]
        public IActionResult login(string email, string pw)
        {
            User user = db.Users.SingleOrDefault( u => u.Email == email );
            if(user != null) 
            {
                var Hasher = new PasswordHasher<User>();
                if(pw == null) pw = "";
                if(Hasher.VerifyHashedPassword(user, user.Password, pw) != 0)
                {
                    HttpContext.Session.SetInt32("UserID", user.UserId);
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
            int id = (int)HttpContext.Session.GetInt32("UserID");
            ViewBag.Errors = TempData["Errors"];
            ViewBag.User = db.Users.Find(id);
            ViewBag.Quotes = db.Quotes
                .OrderByDescending(q => q.Likes)
                .Include( q => q.User );
                // .ToList();
            return View();
        }

        [HttpGet("users")]
        public IEnumerable<User> ShowUsers()
        {
            return db.Users;
        }

        [HttpGet("user/{id}")]
        public IActionResult ShowUser(int id)
        {
            ViewBag.id = (int)HttpContext.Session.GetInt32("UserID");
            ViewBag.user = db.Users.Include( u => u.Quotes ).SingleOrDefault( u => u.UserId == id );
            return View("user");
        }
// ================================================================================

        [HttpPost("quotes")]
        public IActionResult addQuotes(QuoteViewModel model)
        {
            int userID = (int)HttpContext.Session.GetInt32("UserID");
            if(ModelState.IsValid)
            {
                Quote NewQuote = new Quote {
                    Content = model.Content,
                    Likes = 0,
                    UserId = userID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now 
                };
                db.Add(NewQuote);
                db.SaveChanges();
                return RedirectToAction("quotes");
            }
            int id = (int)HttpContext.Session.GetInt32("UserID");
            ViewBag.User = db.Users.Find(id);
            ViewBag.Quotes = db.Quotes
                .OrderByDescending(q => q.Likes)
                .Include( q => q.User );
                // .ToList();
            return View("quotes");
        }
// ================================================================================
        
        [HttpGet("quotes/likes/{id}")]
        public IActionResult updateLikes(int id)
        {
            Quote quote = db.Quotes.Find(id);
            quote.Likes += 1;
            db.SaveChanges();
            return RedirectToAction("quotes");
        }
// ================================================================================

        [HttpGet("quotes/delete/{id}")]
        public IActionResult delete(int id)
        {
            Quote quote = db.Quotes.Find(id);
            db.Quotes.Remove(quote);
            db.SaveChanges();
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
