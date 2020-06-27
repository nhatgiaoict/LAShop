using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.System;
using LACoreApp.Data.Entities;
using LACoreApp.Infrastructure.Interfaces;
using LACoreApp.Utilities.Dtos;

namespace LACoreApp.Application.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepository<Function, Guid> _functionRepository;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IRepository<Announcement, string> _announRepository;
        private readonly IRepository<AnnouncementUser, int> _announUserRepository;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public RoleService(RoleManager<AppRole> roleManager,
            IUnitOfWork unitOfWork,
            IRepository<AnnouncementUser, int> announUserRepository,
         IRepository<Function, Guid> functionRepository,
         IRepository<Permission, int> permissionRepository,
            IRepository<Announcement, string> announRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _announRepository = announRepository;
            _functionRepository = functionRepository;
            _announUserRepository = announUserRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(AnnouncementViewModel announcementVm,
            List<AnnouncementUserViewModel> announcementUsers, AppRoleViewModel roleVm)
        {
            var role = new AppRole()
            {
                Name = roleVm.Name,
                Description = roleVm.Description
            };
            var result = await _roleManager.CreateAsync(role);
            var announcement = _mapper.Map<AnnouncementViewModel, Announcement>(announcementVm);
            _announRepository.Add(announcement);
            foreach (var userVm in announcementUsers)
            {
                var user = _mapper.Map<AnnouncementUserViewModel, AnnouncementUser>(userVm);
                _announUserRepository.Add(user);
            }
            _unitOfWork.Commit();
            return result.Succeeded;
        }

        public Task<bool> CheckPermission(Guid functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _mapper.ProjectTo<AppRoleViewModel>(_roleManager.Roles).ToListAsync();
        }

        public PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = _mapper.ProjectTo<AppRoleViewModel>(query).ToList();
            var paginationSet = new PagedResult<AppRoleViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return _mapper.Map<AppRole, AppRoleViewModel>(role);
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        select new PermissionViewModel()
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanCreate = p != null && p.CanCreate,
                            CanDelete = p != null && p.CanDelete,
                            CanRead = p != null && p.CanRead,
                            CanUpdate = p != null && p.CanUpdate
                        };
            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionVms, Guid roleId)
        {
            var permissions = _mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionVms);
            var oldPermission = _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToList();
            if (oldPermission.Count > 0)
            {
                _permissionRepository.RemoveMultiple(oldPermission);
            }
            foreach (var permission in permissions)
            {
                _permissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public async Task UpdateAsync(AppRoleViewModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            await _roleManager.UpdateAsync(role);
        }
    }
}