using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_BlogProject.Filters;
using MVC_BlogProject.Models;
using MVC_BlogProject.Models.Data;
using MVC_BlogProject.Models.Entity;
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
            List<ArticleViewModel> list =
                _db.Articles
                .Take(20)
                .Select(x => new ArticleViewModel()
                {
                    Id = x.Id,
                    AuthorId = x.AuthorId.ToString(),
                    AuthorName = x.Author.Username,
                    ArticlePicture = string.IsNullOrEmpty(x.ArticlePicture) ? "null.png" : x.ArticlePicture,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime
                }).ToList();

            return View(list);
        }

        [LoggedUser]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("[action]/{username}")]
        public IActionResult Profile(string username)
        {
            List<ArticleViewModel> list =
                _db.Articles
                .Where(x => x.Author.Username.Equals(username))
                .Select(x => new ArticleViewModel()
                {
                    Id = x.Id,
                    AuthorId = x.AuthorId.ToString(),
                    AuthorName = x.Author.Username,
                    ArticlePicture = string.IsNullOrEmpty(x.ArticlePicture) ? "null.png" : x.ArticlePicture,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime
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
