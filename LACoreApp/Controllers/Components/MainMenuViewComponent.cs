using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
using LACoreApp.Data.Enums;
using LACoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.JsonPatch.Adapters;

namespace LACoreApp.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {

        private IMenuService _menuService;
        private IBlogCategoryService _blogCategoryService;
        private IProductCategoryService _productCategoryService;

        public MainMenuViewComponent(IMenuService menuService, IBlogCategoryService blogCategoryService, IProductCategoryService productCategoryService)
        {
            _menuService = menuService;
            _blogCategoryService = blogCategoryService;
            _productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = await _menuService.GetList(filter: "", group: (int)MenuGroup.top, status: Status.Active);
            var blogCategories = _blogCategoryService.GetAll();
            var productCategories = _productCategoryService.GetAll();
            foreach(var item in menus)
            {
                if (item.Type == (int)MenuType.Blog)
                {
                    item.Target = blogCategories.SingleOrDefault(o => o.Id == item.Url.ToInt())?.SeoAlias;
                }
                else if(item.Type== (int)MenuType.Product)
                {
                    item.Target = productCategories.SingleOrDefault(o => o.Id == item.Url.ToInt())?.SeoAlias;
                }
                else
                {
                    item.Target = item.Url;
                }
            }
            return View(menus);
        }
    }
}
