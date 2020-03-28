using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Product;
using LACoreApp.Data.Entities;
using LACoreApp.Data.Enums;
using LACoreApp.Infrastructure.Interfaces;

namespace LACoreApp.Application.Implementation
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private IRepository<BlogCategory, int> _blogCategoryRepository;
        private IRepository<Blog, int> _blogRepository;
        private IUnitOfWork _unitOfWork;

        public BlogCategoryService(IRepository<BlogCategory, int> blogCategoryRepository,
            IRepository<Blog, int> blogRepository,
            IUnitOfWork unitOfWork)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
        }

        public BlogCategoryViewModel Add(BlogCategoryViewModel blogCategoryVm)
        {
            var productCategory = Mapper.Map<BlogCategoryViewModel, BlogCategory>(blogCategoryVm);
            _blogCategoryRepository.Add(productCategory);
            return blogCategoryVm;
        }

        public void Delete(int id)
        {
            _blogCategoryRepository.Remove(id);
        }

        public List<BlogCategoryViewModel> GetAll()
        {
            return _blogCategoryRepository.FindAll().OrderBy(x => x.ParentId)
                 .ProjectTo<BlogCategoryViewModel>().ToList();
        }

        public List<BlogCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _blogCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<BlogCategoryViewModel>().ToList();
            else
                return _blogCategoryRepository.FindAll().OrderBy(x => x.ParentId)
                    .ProjectTo<BlogCategoryViewModel>()
                    .ToList();
        }

        public List<BlogCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _blogCategoryRepository.FindAll(x => x.Status == Status.Active
            && x.ParentId == parentId)
             .ProjectTo<BlogCategoryViewModel>()
             .ToList();
        }

        public BlogCategoryViewModel GetById(int id)
        {
            return Mapper.Map<BlogCategory, BlogCategoryViewModel>(_blogCategoryRepository.FindById(id));
        }

        public List<BlogCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _blogCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Blogs)
                  .OrderBy(x => x.HomeOrder)
                  .Take(top).ProjectTo<BlogCategoryViewModel>();

            var categories = query.ToList();
            foreach (var category in categories)
            {
                category.Blogs = _blogRepository
                    .FindAll(x=>x.CategoryId==category.Id)
                    .OrderByDescending(o => o.DateCreated).Take(5)
                    .ProjectTo<BlogViewModel>().ToList();
            }
            return categories;
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _blogCategoryRepository.FindById(sourceId);
            var target = _blogCategoryRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _blogCategoryRepository.Update(source);
            _blogCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(BlogCategoryViewModel blogCategoryVm)
        {
            var blogCategory = Mapper.Map<BlogCategoryViewModel, BlogCategory>(blogCategoryVm);
            _blogCategoryRepository.Update(blogCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _blogCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _blogCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var sibling = _blogCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _blogCategoryRepository.Update(child);
            }
        }
    }
}
