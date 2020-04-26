using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LACoreApp.Models;
using Microsoft.AspNetCore.Authorization;
using LACoreApp.Extensions;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using LACoreApp.Data.Enums;
using LACoreApp.Data.SeoStructureData;
using LACoreApp.Data.SeoStructureData.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace LACoreApp.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private IBlogCategoryService _blogCategoryService;
        private IConfiguration _configuration;

        private IBlogService _blogService;
        private ICommonService _commonService;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IProductService productService,
        IBlogService blogService, ICommonService commonService,
        IBlogCategoryService blogCategoryService,
       IProductCategoryService productCategoryService, 
        IStringLocalizer<HomeController> localizer,
        IConfiguration configuration)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _blogCategoryService = blogCategoryService;
            _productCategoryService = productCategoryService;
            _localizer = localizer;
            _configuration = configuration;
        }

        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            var title = _localizer["Title"];
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            var homeVm = new HomeViewModel();
            homeVm.HomeBlogCategories = _blogCategoryService.GetHomeCategories(5);
            //homeVm.HotProducts = _productService.GetHotProduct(5);
            //homeVm.TopSellProducts = _productService.GetLastest(6);
            homeVm.LastestBlogs = _blogService.GetLastest(6);
            homeVm.HomeSlides = _commonService.GetSlides(SlideGroup.Home.ToString());

            var webStructure = BuildWebsiteStructured();

            ViewData["WebsiteJson"] = JsonConvert.SerializeObject(BuildWebsiteStructured(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return View(homeVm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        private WebsiteStructureData BuildWebsiteStructured()
        {
            var webStructuredSchema = new WebsiteStructureData()
            {
                Context = "http://schema.org",
                Type = "WebSite",
                Name = _configuration["SystemSettings:WebsiteStruct:Name"],
                AlternateName= _configuration["SystemSettings:WebsiteStruct:AlternateName"],
                Url = _configuration["SystemSettings:Domain"]
            };
            return webStructuredSchema;
        }
    }

}
