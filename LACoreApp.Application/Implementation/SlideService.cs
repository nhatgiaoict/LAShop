using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Data.Entities;
using LACoreApp.Infrastructure.Interfaces;
using LACoreApp.Utilities.Dtos;

namespace LACoreApp.Application.Implementation
{
    public class SlideService : ISlideService
    {
        private IRepository<Slide, int> _slideRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SlideService(IRepository<Slide, int> slideRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._slideRepository = slideRepository;
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Add(SlideViewModel slideVm)
        {
            var slide = _mapper.Map<SlideViewModel, Slide>(slideVm);
            _slideRepository.Add(slide);
        }

        public void Delete(int id)
        {
            _slideRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<SlideViewModel> GetAll()
        {
            return _mapper.ProjectTo<SlideViewModel>(_slideRepository.FindAll()).ToList();
        }

        public PagedResult<SlideViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _slideRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DisplayOrder)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<SlideViewModel>()
            {
                Results = _mapper.ProjectTo<SlideViewModel>(data).ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public SlideViewModel GetById(int id)
        {
            return _mapper.Map<Slide, SlideViewModel>(_slideRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(SlideViewModel slideVm)
        {
            var slide = _mapper.Map<SlideViewModel, Slide>(slideVm);
            _slideRepository.Update(slide);
        }
    }
}
