using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACoreApp.Controllers.Components
{
    public class MostViewsViewComponent : ViewComponent
    {
        private IBlogService _blogService;
        private IMemoryCache _memoryCache;
        public MostViewsViewComponent(IBlogService blogService, IMemoryCache memoryCache)
        {
            _blogService = blogService;
            _memoryCache = memoryCache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = _memoryCache.GetOrCreate(CacheKeys.MostView, entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return _blogService.GetMostView(10);
            });
            return View(blogs);
        }
    }
}
