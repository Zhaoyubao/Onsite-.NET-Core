using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using LogReg.Models;
using LogReg.Factory;

namespace LogReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserFactory userFactory;
        public HomeController(UserFactory user) {
            userFactory = user;
        }
// ================================================================================

        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("main");
            ViewBag.RegErrors = TempData?["RegErrors"] ?? "";
            ViewBag.RegEmailErr = TempData?["RegEmailErr"] ?? "";
            ViewBag.LogPwErr = TempData?["LogPwErr"] ?? "";
            ViewBag.LogEmailErr = TempData?["LogEmailErr"] ?? "";
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
                    return RedirectToAction("main");
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
                    return RedirectToAction("main");
                }
                else TempData["LogPwErr"] = "Incorrect password!";
             }
             else TempData["LogEmailErr"] = "Email address does not exist!";
             return RedirectToAction("index");
         }
// ================================================================================
        
         [HttpGet("main")]
         public IActionResult main()
         {
             if(HttpContext.Session.GetInt32("UserID") == null)
                 return RedirectToAction("index");
             ViewBag.User = userFactory.FindByID((int)HttpContext.Session.GetInt32("UserID"));
             return View();
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
