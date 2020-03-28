using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Data.Enums;
using LACoreApp.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LACoreApp.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        #region Initialize

        private IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
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

        [HttpGet]
        public IActionResult GetAllFillter(string filter)
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
                .Select(c => new SelectListItem()
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                }).ToList();

            return new OkObjectResult(groups);
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