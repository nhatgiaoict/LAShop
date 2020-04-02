using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.ActionFilters;
using LACoreApp.Application.Interfaces;
using LACoreApp.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LACoreApp.Controllers
{
    
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IConfiguration _configuration;
        private readonly IBlogCategoryService _blogCategoryService;

        public BlogController(IBlogService blogService, IConfiguration configuration, IBlogCategoryService blogCategoryService)
        {
            _blogService = blogService;
            _configuration = configuration;
            _blogCategoryService = blogCategoryService;
        }

        [Route("tin-tuc.html", Name = "Blog")]
        public IActionResult Index()
        {
            var categories = _blogCategoryService.GetAll();
            return View(categories);
        }
        [Route("{alias}-bc-{id}.html", Name = "Catalog")]
        public IActionResult Catalog(int id, int? pageSize, int page = 1)
        {
            var catalog = new CatalogViewModel();

            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.Data = _blogService.GetAllPaging(id, string.Empty, page, pageSize.Value);
            catalog.MostViews = _blogService.GetMostView(10);
            catalog.Category = _blogCategoryService.GetById(id);
            return View(catalog);
        }

        [Route("{alias}-b-{id}.html", Name = "BlogDetail")]
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

        [Route("tim-kiem.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new SearchResultViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.Data = _blogService.GetAllPaging(null, keyword, page, pageSize.Value);
            catalog.Keyword = keyword;

            return View(catalog);
        }
    }
}