﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using LACoreApp.Authorization;
using LACoreApp.Data.Enums;
using LACoreApp.Extensions;
using LACoreApp.SignalR;

namespace LACoreApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IFunctionService _functionService;
        private readonly IHubContext<TeduHub> _hubContext;


        public UserController(IUserService userService,
            IAuthorizationService authorizationService,
            IHubContext<TeduHub> hubContext,
            IFunctionService functionService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _hubContext = hubContext;
            _functionService = functionService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, new Guid("1840BD0E-7638-46AE-9A9E-8C34356267DB"), Operations.Read);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Login/Index");

            return View();
        }
        public IActionResult GetAll()
        {
            var model = _userService.GetAllAsync();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await _userService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (userVm.Id == null)
            {
                var announcement = new AnnouncementViewModel()
                {
                    Content = $"User {userVm.UserName} has been created",
                    DateCreated = DateTime.Now,
                    Status = Status.Active,
                    Title = "User created",
                    UserId = User.GetUserId(),
                    Id = Guid.NewGuid().ToString(),
                    
                };
                await _userService.AddAsync(userVm);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            }
            else
            {
                await _userService.UpdateAsync(userVm);
            }
            return new OkObjectResult(userVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _userService.DeleteAsync(id);

                return new OkObjectResult(id);
            }
        }
    }
}