using LACoreApp.Data.Enums;
using LACoreApp.Data.Interfaces;
using LACoreApp.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LACoreApp.Data.Entities
{
    [Table("Menus")]
    public class Menu : DomainEntity<Guid>, ISwitchable, ISortable
    {
        public Menu()
        {

        }
        public Menu(string name, string url, Guid? parentId, int group, int sortOrder, Status status)
        {
            this.Name = name;
            this.URL = url;
            this.ParentId = parentId;
            this.Group = group;
            this.SortOrder = sortOrder;
            this.Status = Status.Active;
        }

        [Required]
        [StringLength(128)]
        public string Name { set; get; }

        [Required]
        [StringLength(250)]
        public string URL { set; get; }

        public Guid? ParentId { get; set; }

        [Required]
        public int Group { get; set; }

        public Status Status { get; set; }
        public int SortOrder { get ; set ; }
    }
}
