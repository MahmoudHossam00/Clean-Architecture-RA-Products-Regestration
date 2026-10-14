using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Products;
using RAProductsRegestration.Domain.PromotionalMaterials;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Data.Configurations
{
    internal sealed class PromotionalMaterialConfiguration
     : IEntityTypeConfiguration<PromotionalMaterial>
    {
        public void Configure(
            EntityTypeBuilder<PromotionalMaterial> builder)
        {
            // =====================================================
            // Table
            // =====================================================

            builder.ToTable(
                "PromotionalMaterials");


            // =====================================================
            // Key
            // =====================================================

            builder.HasKey(
                x => x.Id);


            // =====================================================
            // Product
            // =====================================================

            builder.Property(
                    x => x.ProductId)
                .IsRequired();


            builder.HasIndex(
                x => x.ProductId);


            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(
                    x => x.ProductId)
                .OnDelete(
                    DeleteBehavior.Restrict);


            // =====================================================
            // Promotional Material Type
            // =====================================================

            builder.Property(
                    x => x.PromotionalMaterialTypeId)
                .IsRequired();


            builder.HasIndex(
                x => x.PromotionalMaterialTypeId);


            builder.HasOne<PromotionalMaterialType>()
                .WithMany()
                .HasForeignKey(
                    x => x.PromotionalMaterialTypeId)
                .OnDelete(
                    DeleteBehavior.Restrict);


            // =====================================================
            // Material File
            // =====================================================

            builder.OwnsOne(
                x => x.MaterialFile,
                file =>
                {
                    file.Property(
                            x => x.AttachmentId)
                        .HasColumnName(
                            "MaterialFileAttachmentId")
                        .IsRequired();


                    file.Property(
                            x => x.FileName)
                        .HasColumnName(
                            "MaterialFileName")
                        .HasMaxLength(500)
                        .IsRequired();


                    file.Property(
                            x => x.Notes)
                        .HasColumnName(
                            "MaterialFileNotes")
                        .HasMaxLength(2000);
                });


            // =====================================================
            // Approval
            // =====================================================

            builder.OwnsOne(
                x => x.Approval,
                approval =>
                {
                    approval.Property(
                            x => x.Date)
                        .HasColumnName(
                            "ApprovalDate");


                    approval.OwnsOne(
                        x => x.Attachment,
                        attachment =>
                        {
                            attachment.Property(
                                    x => x.AttachmentId)
                                .HasColumnName(
                                    "ApprovalAttachmentId");


                            attachment.Property(
                                    x => x.FileName)
                                .HasColumnName(
                                    "ApprovalFileName")
                                .HasMaxLength(500);


                            attachment.Property(
                                    x => x.Notes)
                                .HasColumnName(
                                    "ApprovalFileNotes")
                                .HasMaxLength(2000);
                        });
                });


            // =====================================================
            // Approval Expiry
            // =====================================================

            builder.Property(
                x => x.ApprovalExpiryDate);


            builder.HasIndex(
                x => x.ApprovalExpiryDate);
        }
    }
}
