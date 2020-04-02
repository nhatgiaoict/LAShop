using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.ActionFilters;
using LACoreApp.Application.Interfaces;
using LACoreApp.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LACoreApp.Controllers
{
    [UseAmpImage]
    public class AmpController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;

        public AmpController(IBlogService blogService, IBlogCategoryService blogCategoryService)
        {
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
        }
        [Route("amp/{alias}-b-{id}.html", Name = "AmpBlogDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "blog-page";
            var model = new DetailViewModel();
            model.Blog = _blogService.GetById(id);
            //model.Category = _blogCategoryService.GetById(model.Blog.CategoryId ?? 0);
            model.RelatedBlogs = _blogService.GetReatedBlogs(id, 9);
            model.LastestBlogs = _blogService.GetLastest(5);
            // model.Tags = _blogService.GetListTagById(id);
            return View(model);
        }
    }
}