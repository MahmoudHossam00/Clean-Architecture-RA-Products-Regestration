using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Suppliers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Data.Configurations
{
    internal sealed class SupplierConfiguration
    : EntityConfiguration<Supplier>
    {
        protected override void ConfigureEntity(
            EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            //builder.Property(x => x.ExternalId)
            //    .IsRequired();

            builder.Property(x => x.JdeCode)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            //builder.HasIndex(x => x.ExternalId)
            //    .IsUnique();

            builder.HasIndex(x => x.JdeCode)
                .IsUnique();
        }
    }
}
