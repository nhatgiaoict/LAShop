using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Product;
using LACoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LACoreApp.Areas.Admin.Controllers
{
    public class BlogCategoryController : BaseController
    {
        private readonly IBlogCategoryService _blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Get Data API
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _blogCategoryService.GetById(id);
            return new ObjectResult(model);
        }
        [HttpPost]
        public IActionResult SaveEntity(BlogCategoryViewModel blogCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                blogCategoryVm.SeoAlias = TextHelper.ToUnsignString(blogCategoryVm.Name);
                if (blogCategoryVm.Id == 0)
                {
                    _blogCategoryService.Add(blogCategoryVm);
                }
                else
                {
                    _blogCategoryService.Update(blogCategoryVm);
                }
                _blogCategoryService.Save();
                return new OkObjectResult(blogCategoryVm);

            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _blogCategoryService.Delete(id);
                _blogCategoryService.Save();
                return new OkObjectResult(id);
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _blogCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
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
                    _blogCategoryService.UpdateParentId(sourceId, targetId, items);
                    _blogCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult ReOrder(int sourceId, int targetId)
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
                    _blogCategoryService.ReOrder(sourceId, targetId);
                    _blogCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        #endregion
    }
}