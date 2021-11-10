using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_BlogProject.Models.Data;
using MVC_BlogProject.Models.Entity;
using MVC_BlogProject.ViewModels.Auth.Login;
using MVC_BlogProject.ViewModels.Auth.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVC_BlogProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseContext _db;

        public AuthController(DatabaseContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password));

                if (user is not null)
                {
                    HttpContext.Session.SetString("userId", user.Id.ToString());
                    HttpContext.Session.SetString("username", user.Username.ToString());
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User name or password is wrong");
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                Regex r = new Regex(@"^[a - zA - Z]\w{ 8, 14 }$");
                if (_db.Users.Any(x => x.Username.Equals(user.Username)))
                {
                    ModelState.AddModelError("", "There is already a person has same Username");
                }
                else if (!r.IsMatch(user.Password))
                {
                    ModelState.AddModelError("", "Your password is not compatible with necessary requirements.");
                }
                else
                {
                    var newUser = new User() { Username = user.Username, Password = user.Password };
                    _db.Add(newUser);
                    _db.SaveChanges();
                    TempData["message"] = "Register was succesful";
                    return RedirectToAction("Login", "Auth");
                }
            }
            return View();
        }
    }
}
