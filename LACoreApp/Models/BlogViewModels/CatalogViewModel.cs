using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACoreApp.Models.BlogViewModels
{
    public class CatalogViewModel
    {
        public PagedResult<BlogViewModel> Data { get; set; }

        public List<BlogViewModel> MostViews { get; set; }

        public BlogCategoryViewModel Category { set; get; }
    }
}
