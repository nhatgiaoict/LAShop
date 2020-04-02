using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Data.Entities;

namespace LACoreApp.Application.Interfaces
{
    public interface IBlogCategoryService
    {
        BlogCategoryViewModel Add(BlogCategoryViewModel blogCategoryVm);

        void Update(BlogCategoryViewModel blogCategoryVm);

        void Delete(int id);

        List<BlogCategoryViewModel> GetAll();

        List<BlogCategoryViewModel> GetAll(string keyword);

        List<BlogCategoryViewModel> GetAllByParentId(int parentId);

        BlogCategoryViewModel GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        void ReOrder(int sourceId, int targetId);

        List<BlogCategoryViewModel> GetHomeCategories(int top);

        List<BlogCategoryViewModel> GetAllFlat();

        void GetByParentId(IEnumerable<BlogCategory> allItems,
            BlogCategory parent, IList<BlogCategory> items);

        void Save();
    }
}
