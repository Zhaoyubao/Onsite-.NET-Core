using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarRental.Models;

namespace CarRental.Controllers
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
            string username = model.UserName == null ? "" : model.UserName.ToLower();
            User user = db.Users.SingleOrDefault( u => u.UserName == username );
            if(ModelState.IsValid && user == null)
            {
                User NewUser = new User {
                    FullName = model.FullName,
                    UserName = username,
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, model.Password);
                db.Add(NewUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UserID", db.Users.SingleOrDefault( u => u.UserName == username ).UserId);
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
            username = username == null ? "" : username.ToLower();
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

        [HttpGet("admin")]
        public IActionResult Admin()
        {
            if(HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("Main");
            ViewBag.LogPwErr = TempData["LogPwErr"];
            ViewBag.LogUserErr = TempData["LogUserErr"];
            return View();
        }
        
        [HttpPost("admin")]
        public IActionResult AdminLogin(string username, string pw)
        {   
            username = username == null ? "" : username.ToLower();
            User user = db.Users.SingleOrDefault( u => u.UserName == username );
            if(user != null) 
            {
                if(user.UserId != 1)
                {
                    TempData["LogUserErr"] = "You are not Admin User!";
                    return RedirectToAction("Admin");
                }
                var Hasher = new PasswordHasher<User>();
                if(pw == null) pw = "";
                if(Hasher.VerifyHashedPassword(user, user.Password, pw) != 0)
                {
                    HttpContext.Session.SetInt32("UserID", 1);
                    return RedirectToAction("Cars");
                }
                else TempData["LogPwErr"] = "Incorrect password!";
            }
            else TempData["LogUserErr"] = "Username does not exist!";
            return RedirectToAction("Admin");
        }
// ================================================================================

        [HttpGet("main")]
        public IActionResult Main()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            int id = (int)HttpContext.Session.GetInt32("UserID");
            User User = db.Users
                .Include( u => u.Rents )
                    .ThenInclude( r => r.Car )
                    .SingleOrDefault( u => u.UserId == id );
            ViewBag.User = User;
            ViewBag.Rentals = User.Rents.Where( r => DateTime.Compare(r.ReturnDate, DateTime.Now.Date) >= 0 );
            ViewBag.Overdues = User.Rents.Where( r => DateTime.Compare(r.ReturnDate, DateTime.Now.Date) < 0 );
            return View();
        }
// ================================================================================

        [HttpGet("cars")]
        public IActionResult Cars()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost("cars")]
        public IActionResult AddCar(Car newCar)
        {
            if(ModelState.IsValid)
            {
                db.Add(newCar);
                db.SaveChanges();
                return RedirectToAction("dashboard");
            }
            return View("cars");
        }
// ================================================================================

        [HttpGet("dashboard")]
        public IActionResult dashboard()
        {


            return View();
        }
// ================================================================================

        [HttpGet("rent")]
        public IActionResult Rent()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            DateTime now = DateTime.Now;
            ViewBag.Min = $"{now.Year}-{now.Month}-{now.Day}"; 
            ViewBag.Max = $"{now.Year}-{now.Month}-{now.Day+6}"; 
            ViewBag.Cars = db.Cars.OrderBy(c => c.Make);
            return View();
        }

        [HttpPost("rent")]
        public IActionResult AddRental(Rental rent)
        {
            int UserId = (int)HttpContext.Session.GetInt32("UserID");
            rent.UserId = UserId;
            db.Add(rent);
            db.SaveChanges();
            return RedirectToAction("Main");
        }

// ================================================================================

       [HttpGet("logout")]
       public IActionResult Logout()
       {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
       }
// ================================================================================



    }
}