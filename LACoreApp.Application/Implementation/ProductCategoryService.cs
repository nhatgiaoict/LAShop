using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Product;
using LACoreApp.Data.Entities;
using LACoreApp.Data.Enums;
using LACoreApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LACoreApp.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductCategoryService(IRepository<ProductCategory, int> productCategoryRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = _mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productCategoryVm;
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _mapper.ProjectTo<ProductCategoryViewModel>(_productCategoryRepository.FindAll().OrderBy(x => x.ParentId)).ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _mapper.ProjectTo<ProductCategoryViewModel>(_productCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId)).ToList();
            else
                return _mapper.ProjectTo<ProductCategoryViewModel>(_productCategoryRepository.FindAll().OrderBy(x => x.ParentId)).ToList();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _mapper.ProjectTo<ProductCategoryViewModel>(_productCategoryRepository.FindAll(x => x.Status == Status.Active
            && x.ParentId == parentId)).ToList();
        }

        public List<ProductCategoryViewModel> GetAllFlat()
        {
            var productCategories = _productCategoryRepository.FindAll();
            var rootProductCategories = productCategories.Where(c => c.ParentId == null);
            var items = new List<ProductCategory>();
            foreach (var item in rootProductCategories)
            {
                //add the parent category to the item list
                items.Add(item);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(productCategories.ToList(), item, items);
            }
            return _mapper.ProjectTo<ProductCategoryViewModel>(items.AsQueryable()).ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return _mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public void GetByParentId(IEnumerable<ProductCategory> allItems, ProductCategory parent, IList<ProductCategory> items)
        {
            var productCategoryEntities = allItems as ProductCategory[] ?? allItems.ToArray();
            var subProductCategories = productCategoryEntities.Where(c => c.ParentId == parent.Id);
            foreach (var bc in subProductCategories)
            {
                //add this category
                items.Add(bc);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(productCategoryEntities, bc, items);
            }
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _mapper.ProjectTo<ProductCategoryViewModel>(_productCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Products)
                  .OrderBy(x => x.HomeOrder)
                  .Take(top));

            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Products = _productRepository
                //    .FindAll(x => x.HotFlag == true && x.CategoryId == category.Id)
                //    .OrderByDescending(x => x.DateCreated)
                //    .Take(5)
                //    .ProjectTo<ProductViewModel>().ToList();
            }
            return categories;
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = _mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Update(productCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _productCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }
    }
}