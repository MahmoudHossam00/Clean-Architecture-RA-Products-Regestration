using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Data.Configurations
{
    internal sealed class PromotionalMaterialTypeConfiguration
        : IEntityTypeConfiguration<PromotionalMaterialType>
    {
        public void Configure(
            EntityTypeBuilder<PromotionalMaterialType> builder)
        {
            // =====================================================
            // Table
            // =====================================================

            builder.ToTable(
                "PromotionalMaterialTypes");


            // =====================================================
            // Key
            // =====================================================

            builder.HasKey(
                x => x.Id);


            // =====================================================
            // Name
            // =====================================================

            builder.Property(
                    x => x.Name)
                .HasMaxLength(150)
                .IsRequired();


            // =====================================================
            // Unique Name
            // =====================================================

            builder.HasIndex(
                    x => x.Name)
                .IsUnique();
        }
    }
}
