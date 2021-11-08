using Microsoft.AspNetCore.Mvc;
using MVC_BlogProject.Models.Data;
using MVC_BlogProject.ViewModels.Auth.Login;
using MVC_BlogProject.ViewModels.Auth.Register;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.Any(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "There is no user with same Username or Username and Password can not be matched");
                }
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Register(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.Any(x => x.Username.Equals(user.Username)))
                {
                    ModelState.AddModelError("", "There is already a person has same Username");
                }
                else
                {
                    _db.Add(user);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}
