using BeYeuBookstore.Data.EF.Extensions;
using BeYeuBookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Configurations
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
