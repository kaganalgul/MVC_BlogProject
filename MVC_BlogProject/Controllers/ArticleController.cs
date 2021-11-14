using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_BlogProject.Managers;
using MVC_BlogProject.Filters;
using MVC_BlogProject.Models.Data;
using MVC_BlogProject.Models.Entity;
using MVC_BlogProject.ViewModels.Article.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_BlogProject.Controllers
{
    public class ArticleController : Controller
    {
        private readonly FileManager _fileManager;
        private readonly DatabaseContext _db;

        public ArticleController(IWebHostEnvironment webHostEnvironment, DatabaseContext context)
        {
            _fileManager = new FileManager(webHostEnvironment);
            _db = context;
        }

        [LoggedUser]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article()
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = int.Parse(HttpContext.Session.GetString("userId")),
                    ArticlePicture = _fileManager.GetUniqueNameAndSavePhotoToDisk(model.ArticlePicture)
                };
                _db.Articles.Add(article);
                _db.SaveChanges();
            }
            TempData["message"] = "Tebrikler Article Oluşturma Başarılı.";
            return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
        }
    }
}
