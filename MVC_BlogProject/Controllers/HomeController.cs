using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_BlogProject.Models;
using MVC_BlogProject.Models.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _db;

        public HomeController(DatabaseContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Users.First());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
