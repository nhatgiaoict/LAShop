using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.System;
using LACoreApp.Data.Entities;
using LACoreApp.Data.Enums;
using LACoreApp.Infrastructure.Interfaces;

namespace LACoreApp.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        private readonly IRepository<Function, Guid> _functionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FunctionService(IMapper mapper,
            IRepository<Function, Guid> functionRepository,
            IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CheckExistedId(Guid id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Add(FunctionViewModel functionVm)
        {
            var function = _mapper.Map<Function>(functionVm);
            _functionRepository.Add(function);
        }

        public void Delete(Guid id)
        {
            _functionRepository.Remove(id);
        }

        public FunctionViewModel GetById(Guid id)
        {
            var function = _functionRepository.FindSingle(x => x.Id == id);
            return Mapper.Map<Function, FunctionViewModel>(function);
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));
            return query.OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder).ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public IEnumerable<FunctionViewModel> GetAllWithParentId(Guid parentId)
        {
            return _functionRepository.FindAll(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionVm)
        {
            //var functionDb = _functionRepository.FindById(functionVm.Id);
            var function = _mapper.Map<FunctionViewModel, Function>(functionVm);
            _functionRepository.Update(function);
        }

        public void ReOrder(Guid sourceId, Guid targetId)
        {
            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _functionRepository.Update(source);
            _functionRepository.Update(target);
        }

        public void UpdateParentId(Guid sourceId, Guid targetId, Dictionary<Guid, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //Get all sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}