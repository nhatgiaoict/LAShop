using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LACoreApp.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        public ISlideService _slideService;
        public IActionResult Index()
        {
            return View();
        }
        public SlideController(ISlideService slideService)
        {
            _slideService = slideService;
        }
        public IActionResult GetAll()
        {
            var model = _slideService.GetAll();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _slideService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _slideService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(SlideViewModel slideVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (slideVm.Id == 0)
            {
                _slideService.Add(slideVm);
            }
            else
            {
                _slideService.Update(slideVm);
            }
            _slideService.SaveChanges();
            return new OkObjectResult(slideVm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _slideService.Delete(id);
            _slideService.SaveChanges();

            return new OkObjectResult(id);
        }
    }
}