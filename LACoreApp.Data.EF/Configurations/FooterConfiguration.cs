﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Data.EF.Extensions;
using LACoreApp.Data.Entities;

namespace LACoreApp.Data.EF.Configurations
{
   public class FooterConfiguration : DbEntityConfiguration<Footer>
    {
        public override void Configure(EntityTypeBuilder<Footer> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255)
                .IsUnicode(false).HasMaxLength(255).IsRequired();
            // etc.
        }
    }
}
