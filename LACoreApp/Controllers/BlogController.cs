using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
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

        [Route("blogs.html")]
        public IActionResult Index()
        {
            var categories = _blogCategoryService.GetAll();
            return View(categories);
        }
    }
}