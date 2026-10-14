using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Data.Configurations
{
    internal sealed class MiscellaneousApprovalFileTypeConfiguration
    : IEntityTypeConfiguration<MiscellaneousApprovalFileType>
    {
        public void Configure(
            EntityTypeBuilder<MiscellaneousApprovalFileType> builder)
        {
            // =====================================================
            // Table
            // =====================================================

            builder.ToTable(
                "MiscellaneousApprovalFileTypes");


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


            builder.HasIndex(
                    x => x.Name)
                .IsUnique();
        }
    }
}
