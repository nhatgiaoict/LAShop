using AutoMapper;
using AutoMapper.QueryableExtensions;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Application.ViewModels.System;
using LACoreApp.Data.Entities;
using LACoreApp.Data.Enums;
using LACoreApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACoreApp.Application.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu, Guid> _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuService(IMapper mapper,
            IRepository<Menu, Guid> menuRepository,
            IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Add(MenuViewModel menuVm)
        {
            var menu = _mapper.Map<Menu>(menuVm);
            _menuRepository.Add(menu);
        }

        public bool CheckExistedId(Guid id)
        {
            return _menuRepository.FindById(id) != null;
        }

        public void Delete(Guid id)
        {
            _menuRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public Task<List<MenuViewModel>> GetList(string filter, Guid? parentId = null, int? group = null, Status? status = null)
        {
            var query = _menuRepository.FindAll()
                                       .Where(x => string.IsNullOrEmpty(filter) || x.Name.Contains(filter))
                                       .Where(x => !parentId.HasValue || x.ParentId == parentId)
                                       .Where(x => !group.HasValue || x.Group == group)
                                       .Where(x => !status.HasValue || x.Status == status);
            return _mapper.ProjectTo<MenuViewModel>(query.OrderBy(x => x.SortOrder)).ToListAsync();

        }

        public IEnumerable<MenuViewModel> GetAllWithParentId(Guid parentId)
        {
            return _mapper.ProjectTo<MenuViewModel>(_menuRepository.FindAll(x => x.ParentId == parentId)
                                  .OrderBy(x => x.SortOrder));
        }

        public MenuViewModel GetById(Guid id)
        {
            var menu = _menuRepository.FindSingle(x => x.Id == id);
            return _mapper.Map<Menu, MenuViewModel>(menu);
        }



        public void ReOrder(Guid sourceId, Guid targetId)
        {
            var source = _menuRepository.FindById(sourceId);
            var target = _menuRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _menuRepository.Update(source);
            _menuRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(MenuViewModel menuVm)
        {
            var menu = _mapper.Map<MenuViewModel, Menu>(menuVm);
            _menuRepository.Update(menu);
        }

        public void UpdateParentId(Guid sourceId, Guid targetId, Dictionary<Guid, int> items)
        {
            var category = _menuRepository.FindById(sourceId);
            category.ParentId = targetId;
            _menuRepository.Update(category);

            //Get all sibling
            var sibling = _menuRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _menuRepository.Update(child);
            }
        }
    }
}
