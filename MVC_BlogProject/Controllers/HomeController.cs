using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_BlogProject.Models;
using MVC_BlogProject.Models.Data;
using MVC_BlogProject.ViewModels.Home.Profile;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            List<ArticleViewModel> list =
                _db.Articles
                .Where(x => x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")))
                .Select(x => new ArticleViewModel()
                {
                    Id = x.Id,
                    AuthorId = x.AuthorId.ToString(),
                    AuthorName = x.Author.Username,
                    ArticlePicture = x.ArticlePicture,
                    Title = x.Title,
                    Content = x.Content,
                    // todo CreatedTime Eklenecek.
                }).ToList();
            return View(list);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
