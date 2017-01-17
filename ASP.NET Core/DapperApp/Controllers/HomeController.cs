using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DapperApp.Models;
using DapperApp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;


namespace DapperApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserRepository userRepository;

        public HomeController(UserRepository user) {
            userRepository = user;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index", userRepository.FindAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RouteAttribute("new")]
        public IActionResult New(User newUser)
        {
            userRepository.Add(newUser);
            return RedirectToAction("Index");
        }
    }
}
