using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Data.EF.Extensions;
using LACoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LACoreApp.Data.EF.Configurations
{
    public class AdvertisementPageConfiguration : DbEntityConfiguration<AdvertisementPage>
    {
        public override void Configure(EntityTypeBuilder<AdvertisementPage> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(20).IsRequired().IsUnicode(false);
            // etc.
        }
    }
}
