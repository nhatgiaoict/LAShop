using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACoreApp.Models.CommonViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int SortOrder { get; set; }
    }
}
