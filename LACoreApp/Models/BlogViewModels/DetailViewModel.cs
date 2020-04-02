using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACoreApp.Models.BlogViewModels
{
    public class DetailViewModel
    {
        public BlogViewModel Blog { get; set; }

        public bool Available { set; get; }

        public List<BlogViewModel> RelatedBlogs { get; set; }

        public BlogCategoryViewModel Category { get; set; }

        public List<BlogViewModel> LastestBlogs { get; set; }

        public List<TagViewModel> Tags { set; get; }

    }
}
