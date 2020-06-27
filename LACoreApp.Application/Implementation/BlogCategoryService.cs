using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Product;
using LACoreApp.Data.Entities;
using LACoreApp.Data.Enums;
using LACoreApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LACoreApp.Application.Implementation
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IRepository<BlogCategory, int> _blogCategoryRepository;
        private readonly IRepository<Blog, int> _blogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BlogCategoryService(IRepository<BlogCategory, int> blogCategoryRepository,
            IRepository<Blog, int> blogRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BlogCategoryViewModel Add(BlogCategoryViewModel blogCategoryVm)
        {
            var productCategory = _mapper.Map<BlogCategoryViewModel, BlogCategory>(blogCategoryVm);
            _blogCategoryRepository.Add(productCategory);
            return blogCategoryVm;
        }

        public void Delete(int id)
        {
            _blogCategoryRepository.Remove(id);
        }

        public List<BlogCategoryViewModel> GetAll()
        {
            return _mapper.ProjectTo<BlogCategoryViewModel>(_blogCategoryRepository.FindAll().OrderBy(x => x.ParentId)).ToList();
        }

        public List<BlogCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _mapper.ProjectTo<BlogCategoryViewModel>(_blogCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId)).ToList();
            else
                return _mapper.ProjectTo<BlogCategoryViewModel>(_blogCategoryRepository.FindAll().OrderBy(x => x.ParentId)).ToList();
        }

        public List<BlogCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _mapper.ProjectTo<BlogCategoryViewModel>(_blogCategoryRepository.FindAll(x => x.Status == Status.Active
            && x.ParentId == parentId)).ToList();
        }

        public List<BlogCategoryViewModel> GetAllFlat()
        {
            var blogCategories = _blogCategoryRepository.FindAll();
            var rootBlogCategories = blogCategories.Where(c => c.ParentId == null);
            var items = new List<BlogCategory>();
            foreach (var item in rootBlogCategories)
            {
                //add the parent category to the item list
                items.Add(item);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(blogCategories.ToList(), item, items);
            }
            return _mapper.ProjectTo<BlogCategoryViewModel>(items.AsQueryable()).ToList();
        }

        public BlogCategoryViewModel GetById(int id)
        {
            return _mapper.Map<BlogCategory, BlogCategoryViewModel>(_blogCategoryRepository.FindById(id));
        }

        public List<BlogCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _mapper.ProjectTo<BlogCategoryViewModel>(_blogCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Blogs)
                  .OrderBy(x => x.HomeOrder)
                  .Take(top));

            var categories = query.ToList();
            foreach (var category in categories)
            {
                category.Blogs = _mapper.ProjectTo<BlogViewModel>(_blogRepository
                    .FindAll(x=>x.CategoryId==category.Id)
                    .OrderByDescending(o => o.DateCreated).Take(5)).ToList();
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
            var blogCategory = _mapper.Map<BlogCategoryViewModel, BlogCategory>(blogCategoryVm);
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

        public void GetByParentId(IEnumerable<BlogCategory> allItems,
            BlogCategory parent, IList<BlogCategory> items)
        {
            var blogCategoryEntities = allItems as BlogCategory[] ?? allItems.ToArray();
            var subBlogCategories = blogCategoryEntities.Where(c => c.ParentId == parent.Id);
            foreach (var bc in subBlogCategories)
            {
                //add this category
                items.Add(bc);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(blogCategoryEntities, bc, items);
            }
        }
    }
}
