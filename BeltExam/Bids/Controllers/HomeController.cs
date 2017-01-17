using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeltExam.Models;

namespace BeltExam.Controllers
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
                return RedirectToAction("main");
            ViewBag.LogPwErr = TempData["LogPwErr"];
            ViewBag.LogUserErr = TempData["LogUserErr"];
            return View();
        }
// ================================================================================

        [HttpPost("register")]
        public IActionResult Register(UserViewModel model)
        {
            User user = db.Users.SingleOrDefault( u => u.UserName == model.UserName );
            if(ModelState.IsValid && user == null)
            {
                User NewUser = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, model.Password);
                db.Add(NewUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UserID", db.Users.SingleOrDefault( u => u.UserName == model.UserName ).UserId);
                return RedirectToAction("Main");
            }
            else if(user != null)
                ViewBag.RegUserErr = "Username already exists!";
            return View("index");
        }
// ================================================================================

        [HttpPost("login")]
        public IActionResult Login(string username, string pw)
        {   
            User user = db.Users.SingleOrDefault( u => u.UserName == username );
            if(user != null) 
            {
                var Hasher = new PasswordHasher<User>();
                if(pw == null) pw = "";
                if(Hasher.VerifyHashedPassword(user, user.Password, pw) != 0)
                {
                    HttpContext.Session.SetInt32("UserID", user.UserId);
                    return RedirectToAction("Main");
                }
                else TempData["LogPwErr"] = "Incorrect password!";
            }
            else TempData["LogUserErr"] = "Username does not exist!";
            return RedirectToAction("index");
        }
// ================================================================================

        [HttpGet("main")]
        public IActionResult Main()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            int id = (int)HttpContext.Session.GetInt32("UserID");
            ViewBag.User = db.Users.Find(id);
            var Ended = db.Auctions
                .Where( a => DateTime.Compare(a.EndDate, DateTime.Now.Date) < 0 )
                .Include( a => a.User ).ToList();
            foreach(var a in Ended)
            {
                User user = db.Users.SingleOrDefault( u => u.UserName == a.Bidder );
                user.Wallet -= a.Bid;
                a.User.Wallet += a.Bid;
                a.IsEnded = 1;
                user.UpdatedAt = a.User.UpdatedAt = DateTime.Now;
                db.SaveChanges();
            }
            ViewBag.Auctions = db.Auctions
                .Where( a => a.IsEnded == 0 )
                .OrderBy( a => a.EndDate - DateTime.Now.Date )
                .Include( a => a.User );
            return View();
        }
// ================================================================================

        [HttpGet("auctions")]
        public IActionResult Auction()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            DateTime now = DateTime.Now;
            ViewBag.Date = $"{now.Year}-{now.Month}-{now.Day}";
            return View();
        }

        [HttpPost("auctions")]
        public IActionResult AddAuction(Auction auction)
        {
            if(ModelState.IsValid)
            {
                int UserId = (int)HttpContext.Session.GetInt32("UserID");
                User user = db.Users.Find(UserId);
                auction.UserId = UserId;
                auction.Bidder = user.UserName;
                db.Add(auction);
                db.SaveChanges();
                return RedirectToAction("main");
            }
            DateTime now = DateTime.Now;
            ViewBag.Date = $"{now.Year}-{now.Month}-{now.Day}";
            return View("auction");
        }

        [HttpGet("auctions/delete/{id}")]
        public IActionResult DelAuction(int id)
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            Auction auction = db.Auctions.Find(id);
            db.Auctions.Remove(auction);
            db.SaveChanges();
            return RedirectToAction("Main");
        }
// ================================================================================

        [HttpGet("products/{id}")]
        public IActionResult Product(int id)
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            Auction Product = db.Auctions
                .Include( a => a.User )
                .SingleOrDefault( a => a.AuctionId == id );
            User Bidder = db.Users
                .SingleOrDefault( u => u.UserName == Product.Bidder );
            ViewBag.Product = Product;
            ViewBag.Bidder = $"{Bidder.FirstName} {Bidder.LastName}";
            ViewBag.Err = TempData["Err"];
            return View();
        }

        [HttpPost("products/{id}")]
        public IActionResult UpdateBid(int id, float Bid)
        {
            Auction prod = db.Auctions.Find(id);
            int UserId = (int)HttpContext.Session.GetInt32("UserID");
            User user = db.Users.Find(UserId);
            if(Bid == 0.0F)
                TempData["Err"] = "Please input your bid!";
            else if(Bid <= prod.Bid)
                TempData["Err"] = "Your bid must be higher than the previous one!";
            else if(Bid > user.Wallet)
                TempData["Err"] = "You don't have enough cash!";
            else
            {
                prod.Bid = Bid;
                prod.Bidder = user.UserName;
                prod.UpdatedAt = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Main");
            }
            return RedirectToAction("Product");
        }
// ================================================================================

       [HttpGet("logout")]
       public IActionResult Logout()
       {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
       }

    }
}