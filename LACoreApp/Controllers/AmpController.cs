using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.ActionFilters;
using LACoreApp.Application.Interfaces;
using LACoreApp.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace LACoreApp.Controllers
{
    [UseAmpImage]
    public class AmpController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IConfiguration _configuration;

        public AmpController(IBlogService blogService, IBlogCategoryService blogCategoryService, IConfiguration configuration)
        {
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _configuration = configuration;
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
            ViewData["canonical"] = _configuration["SystemSettings:Domain"] + "/" + model.Blog.SeoAlias + "-b-" + model.Blog.Id + ".html";
            return View(model);
        }
    }
}