using ERPWebApp.Data.EF.Extensions;
using ERPWebApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Data.EF.Configurations
{
    public class FunctionConfiguration : DbEntityConfiguration<Function>
    {
        public override void Configure(EntityTypeBuilder<Function> entity)
        {
            entity.HasKey(c => c.KeyId);
            entity.Property(c => c.KeyId).IsRequired()
            .HasColumnType("varchar(128)");
            // etc.
        }

    }
}
