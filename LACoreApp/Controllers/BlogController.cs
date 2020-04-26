using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.ActionFilters;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Data.SeoStructureData;
using LACoreApp.Data.SeoStructureData.Common;
using LACoreApp.Models.BlogViewModels;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            var categories = _blogCategoryService.GetAll();
            return View(categories);
        }
        [Route("{alias}-bc-{id}.html", Name = "Catalog")]
        [ResponseCache(CacheProfileName = "Default")]
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
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "blog-page";
            var model = new DetailViewModel();
            model.Blog = _blogService.GetById(id);
            model.Category = _blogCategoryService.GetById(model.Blog.CategoryId ?? 0);
            model.RelatedBlogs = _blogService.GetReatedBlogs(id, 9);
            model.LastestBlogs = _blogService.GetLastest(5);
            // model.Tags = _blogService.GetListTagById(id);

            ViewData["amphtml"] = _configuration["SystemSettings:Domain"] + "/amp/" + model.Blog.SeoAlias + "-b-" + model.Blog.Id + ".html";

            var articleStructuredSchema = BuildArticleStructured(model.Blog);
            var breadCrumbStructuredData = BuildBreadCrumbDetailStructured(model.Blog, model.Category);

            ViewData["ArticleJson"] = JsonConvert.SerializeObject(articleStructuredSchema, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            ViewData["BreadCrumbJson"] = JsonConvert.SerializeObject(breadCrumbStructuredData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

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

        private ArticleStructData BuildArticleStructured(BlogViewModel blog)
        {
            var articleStructuredSchema = new ArticleStructData()
            {
                Context = "http://schema.org",
                Type = "NewsArticle",
                MainEntityOfPage = new MainEntityOfPage
                {
                    Type = "WebPage",
                    Id = _configuration["SystemSettings:Domain"] + "/" + blog.SeoAlias + "-b-" + blog.Id + ".html"
                },
                Headline = blog.Name,
                Description = blog.Description,
                Image = new Image
                {
                    Type = "ImageObject",
                    Url = _configuration["SystemSettings:Domain"] + blog.Image,
                    With = 720,
                    Height = 480
                },
                DatePublished = blog.DateCreated,
                LastUpdated = blog.DateModified,
                Author = new Author
                {
                    Type = _configuration["SystemSettings:ArticleStruct:AuthorType"],
                    Name = _configuration["SystemSettings:ArticleStruct:Author"]
                },
                Publisher = new Publisher
                {
                    Type = _configuration["SystemSettings:ArticleStruct:PublisherType"],
                    Name = _configuration["SystemSettings:ArticleStruct:Publisher"],
                    Logo = new Logo
                    {
                        Type = "ImageObject",
                        Url = _configuration["SystemSettings:Domain"] + _configuration["SystemSettings:ArticleStruct:Logo"]
                    }
                },
            };
            return articleStructuredSchema;
        }

        private BreadCrumbListStructureData BuildBreadCrumbDetailStructured(BlogViewModel blog, BlogCategoryViewModel category)
        {
            var itemlistElements = new List<ItemListElement>
            {
                new ItemListElement
                {
                    Type = "ListItem",
                    Position = 1,
                    Item = new Item
                    {
                        Id = _configuration["SystemSettings:Domain"] + "/" + category.SeoAlias + "-bc-" + category.Id +
                             ".html",
                        Name = category.Name
                    }
                }
            };
            var bcStructuredSchema = new BreadCrumbListStructureData()
            {
                Context = "http://schema.org",
                Type = "BreadcrumbList",
                ItemListElements = itemlistElements
            };
            return bcStructuredSchema;
        }

        //private BreadCrumbListStructureData BuildBreadCrumbCatalogStructure()
        //{
        //    var itemlistElements = new List<ItemListElement>
        //    {
        //        new ItemListElement
        //        {
        //            Type = "ListItem",
        //            Position = 1,
        //            Item = new Item
        //            {
        //                Id = _configuration["SystemSettings:Domain"],
        //                Name = category.Name
        //            }
        //        }
        //    };
        //    var bcStructuredSchema = new BreadCrumbListStructureData()
        //    {
        //        Context = "http://schema.org",
        //        Type = "BreadcrumbList",
        //        ItemListElements = itemlistElements
        //    };
        //    return bcStructuredSchema;
        //}
    }

}