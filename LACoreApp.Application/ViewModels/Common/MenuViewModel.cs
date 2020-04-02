using LACoreApp.Data.Enums;
using LACoreApp.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LACoreApp.Application.ViewModels.Common
{
    public class MenuViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { set; get; }

        [StringLength(250)]
        public string Url { set; get; }

        public Guid? ParentId { get; set; }

        [Required]
        public int Group { get; set; }

        public int SortOrder { get; set; }

        public Status Status { get; set; }

        public int Type { get; set; }

        public int CategoryId
        {
            get
            {
                if (Type == (int)MenuType.Customize) return 0;
                return Url.ToInt();
            }
        }
        public string Target { get; set; }
    }
}
