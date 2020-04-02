using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Data.Enums;
using LACoreApp.Models.CommonViewModels;
using LACoreApp.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LACoreApp.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        #region Initialize

        private readonly IMenuService _menuService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IProductCategoryService _productCategoryService;

        public MenuController(IMenuService menuService, IBlogCategoryService blogCategoryService, IProductCategoryService productCategoryService)
        {
            _menuService = menuService;
            _blogCategoryService = blogCategoryService;
            _productCategoryService = productCategoryService;
        }

        #endregion Initialize
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _menuService.GetList(string.Empty);
            var rootMenu = model.Where(c => c.ParentId == null);
            var items = new List<MenuViewModel>();
            foreach (var menu in rootMenu)
            {
                //add the parent category to the item list
                items.Add(menu);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), menu, items);
            }
            return new ObjectResult(items);
        }

        public IActionResult GetCategory(int type)
        {
            var items = new List<CategoryViewModel>();
            if (type == (int)MenuType.Blog)
            {
                var blogCategories = _blogCategoryService.GetAllFlat();
                foreach(var blCategory in blogCategories)
                {
                    var categoryViewModel = new CategoryViewModel() {
                        Id = blCategory.Id,
                        Name = blCategory.Name,
                        SortOrder = blCategory.SortOrder,
                        ParentId = blCategory.ParentId
                    };
                    items.Add(categoryViewModel);
                }
            }
            else if(type == (int)MenuType.Product)
            {
                var productCategories = _productCategoryService.GetAllFlat();
                foreach(var pCate in productCategories)
                {
                    var categoryViewModel = new CategoryViewModel()
                    {
                        Id = pCate.Id,
                        Name = pCate.Name,
                        SortOrder = pCate.SortOrder,
                        ParentId = pCate.ParentId
                    };
                    items.Add(categoryViewModel);
                }
            }
            return new ObjectResult(items);
        }

        [HttpGet]
        public IActionResult GetAllFilter(string filter)
        {
            var model = _menuService.GetList(filter);
            return new ObjectResult(model);
        }
        public IActionResult GetById(Guid id)
        {
            var model = _menuService.GetById(id);

            return new ObjectResult(model);
        }

        public IActionResult GetGroups()
        {
            var groups = ((MenuGroup[])Enum.GetValues(typeof(MenuGroup)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.ToString()
                }).ToList();

            return new OkObjectResult(groups);
        }

        public IActionResult GetTypes()
        {
            var types = ((MenuType[])Enum.GetValues(typeof(MenuType)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.ToString()
                }).ToList();

            return new OkObjectResult(types);
        }
        [HttpPost]
        public IActionResult SaveEntity(MenuViewModel menuVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (menuVm.Id.Equals(Guid.Empty))
                {
                    _menuService.Add(menuVm);
                }
                else
                {
                    _menuService.Update(menuVm);
                }
                _menuService.Save();
                return new OkObjectResult(menuVm);
            }
        }
        [HttpPost]
        public IActionResult UpdateParentId(Guid sourceId, Guid targetId, Dictionary<Guid, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _menuService.UpdateParentId(sourceId, targetId, items);
                    _menuService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult ReOrder(Guid sourceId, Guid targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId.Equals(targetId))
                {
                    return new BadRequestResult();
                }
                else
                {
                    _menuService.ReOrder(sourceId, targetId);
                    _menuService.Save();
                    return new OkObjectResult(sourceId);
                }
            }
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            else
            {
                _menuService.Delete(id);
                _menuService.Save();
                return new OkObjectResult(id);
            }
        }
        #region Private Functions
        private void GetByParentId(IEnumerable<MenuViewModel> allMenus,
            MenuViewModel parent, IList<MenuViewModel> items)
        {
            var menusEntities = allMenus as MenuViewModel[] ?? allMenus.ToArray();
            var subMenus = menusEntities.Where(c => c.ParentId == parent.Id);
            foreach (var cat in subMenus)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(menusEntities, cat, items);
            }
        }
        #endregion
    }
}