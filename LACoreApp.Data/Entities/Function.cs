using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LACoreApp.Data.Enums;
using LACoreApp.Data.Interfaces;
using LACoreApp.Infrastructure.SharedKernel;

namespace LACoreApp.Data.Entities
{
    [Table("Functions")]
    public class Function : DomainEntity<Guid>, ISwitchable, ISortable
    {
        public Function()
        {

        }
        public Function(string name, string url, Guid? parentId, string iconCss, int sortOrder)
        {
            //this.Id = id;
            this.Name = name;
            this.URL = url;
            this.ParentId = parentId;
            this.IconCss = iconCss;
            this.SortOrder = sortOrder;
            this.Status = Status.Active;
        }

        [Required]
        [StringLength(128)]
        public string Name { set; get; }

        [Required]
        [StringLength(250)]
        public string URL { set; get; }

        public Guid? ParentId { set; get; }

        public string IconCss { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}
