using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Data.EF.Extensions;
using LACoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LACoreApp.Data.EF.Configurations
{
    public class AnnouncementConfiguration : DbEntityConfiguration<Announcement>
    {
        public override void Configure(EntityTypeBuilder<Announcement> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(128).IsRequired().IsUnicode(false);
            // etc.
        }
    }
}
