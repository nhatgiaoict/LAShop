using LACoreApp.Data.Enums;
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

        [Required]
        [StringLength(250)]
        public string URL { set; get; }

        public Guid? ParentId { get; set; }

        [Required]
        public int Group { get; set; }

        public int SortOrder { get; set; }

        public Status Status { get; set; }
    }
}
