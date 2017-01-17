using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Wall.Models;
using Wall.Factory;

namespace Wall.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserFactory userFactory;
        private readonly MessageFactory messageFactory;
        private readonly CommentFactory commentFactory;
        public HomeController(UserFactory user, MessageFactory msg, CommentFactory com) {
            userFactory = user;
            messageFactory = msg;
            commentFactory = com;
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
        [ValidateAntiForgeryToken]
        public IActionResult register(User NewUser)
        {
            User user = userFactory.FindByEmail(NewUser.Email);
            if(ModelState.IsValid && user == null)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                userFactory.Add(NewUser);
                HttpContext.Session.SetInt32("UserID", userFactory.FindByEmail(NewUser.Email).Id);
                return RedirectToAction("wall");
            }
            else if(user != null)
                ViewBag.RegEmailErr = "Email already exists!";
            return View("index");
        }
// ================================================================================

        [HttpPost("login")]
        public IActionResult login(string email, string pw)
        {
            User user = userFactory.FindByEmail(email);
            if(user != null) 
            {
                var Hasher = new PasswordHasher<User>();
                if(pw == null) pw = "";
                if(Hasher.VerifyHashedPassword(user, user.Password, pw) != 0)
                {
                    HttpContext.Session.SetInt32("UserID", user.Id);
                    return RedirectToAction("wall");
                }
                TempData["LogPwErr"] = "Incorrect password!";
            }
            else TempData["LogEmailErr"] = "Email address does not exist!";
            return RedirectToAction("index");
        }
// ================================================================================
        
        [HttpGet("wall")]
        public IActionResult wall()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("index");
            ViewBag.Now = DateTime.Now;
            ViewBag.User = userFactory.FindByID((int)HttpContext.Session.GetInt32("UserID"));
            ViewBag.Errors = TempData["Errors"];
            ViewBag.Err = TempData["Err"];
            ViewBag.Messages = messageFactory.FindAll();
            ViewBag.Comments = commentFactory.FindAll();
            return View();
        }

// ================================================================================

        [HttpPost("messages")]
        public IActionResult AddMsg(Message msg)
        {
            int userID = (int)HttpContext.Session.GetInt32("UserID");
            if(String.IsNullOrWhiteSpace(msg.Content))
                TempData["Errors"] = "Please enter your message!";
            else 
                messageFactory.Add(msg, userID);
            return RedirectToAction("wall");
        }
// ================================================================================
        
       [HttpPost("comments/{msgID}")]
        public IActionResult AddComment(Comment com, int msgID)
        {
            int userID = (int)HttpContext.Session.GetInt32("UserID");
            if(String.IsNullOrWhiteSpace(com.Content))
                TempData["Errors"] = "Please enter your comment!";
            else
                commentFactory.Add(com, userID, msgID);
            return RedirectToAction("wall");
        }
// ================================================================================

        [HttpGet("messages/delete/{id}")]
        public IActionResult DelMsg(int id)
        {
            Message msg = messageFactory.FindByID(id);
            TimeSpan msgSpan = DateTime.Now - msg.Created_at;
            if(msgSpan.TotalMinutes <= 30)
            {
                messageFactory.DelComsByID(id);
                messageFactory.DelByID(id);
            }
            else TempData["err"] = "You're not allowed to delete your message that was made more than 30 minutes!";
            return RedirectToAction("wall");
        }
// ================================================================================

        [HttpGet("comments/delete/{id}")]
        public IActionResult DelComment(int id)
        {
            Comment com = commentFactory.FindByID(id);
            TimeSpan comSpan = DateTime.Now - com.Created_at;
            if(comSpan.TotalMinutes <= 30)
                commentFactory.DelByID(id);
            else TempData["err"] = "You're not allowed to delete your comment that was made more than 30 minutes!";
            return RedirectToAction("wall");
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
