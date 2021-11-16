using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_BlogProject.Managers;
using MVC_BlogProject.ViewModels.Article.Edit;
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
                TempData["message"] = "Article Created.";
                return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
            }
            else
            {
                return View(model);
            }
        }

        [LoggedUser]
        public IActionResult Edit(int id) 
        {
            var article = _db.Articles.FirstOrDefault(x => x.Id.Equals(id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));

            if (article is not null)
            {
                return View(new EditViewModel() { Id = article.Id, Title = article.Title, Content = article.Content, ArticlePictureName = article.ArticlePicture });
            }
            else
            {
                TempData["error"] = "Data could not find";
                return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = _db.Articles.FirstOrDefault(x => x.Id.Equals(model.Id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));
                if (article is null)
                {
                    ViewData["error"] = "Edit Failed.";
                    return View(model);
                }

                article.Title = model.Title;
                article.Content = model.Content;
                if (model.ArticlePicture is not null)
                {
                    article.ArticlePicture = _fileManager.GetUniqueNameAndSavePhotoToDisk(model.ArticlePicture);
                    _fileManager.RemoveImageFromDisk(model.ArticlePictureName);
                }

                _db.SaveChanges();

                TempData["message"] = "Article Editing Completed.";
                return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
            }
            return View(model);
        }

        [HttpGet]
        [LoggedUser]
        public IActionResult Delete(int id)
        {
            var article = _db.Articles.FirstOrDefault(x => x.Id.Equals(id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));
            if (article is not null)
            {
                _db.Articles.Remove(article);
                _db.SaveChanges();
                _fileManager.RemoveImageFromDisk(article.ArticlePicture);
                TempData["message"] = "Delete completed";
            }
            else
            {
                TempData["error"] = "Data could not find.";
            }
            return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
        }
    }
}
