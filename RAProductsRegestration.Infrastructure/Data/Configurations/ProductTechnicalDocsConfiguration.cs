using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Products;
using RAProductsRegestration.Domain.ProductsTechnicalDocs;
using RAProductsRegestration.Domain.ProductsTechnicalDocs.Common;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class ProductTechnicalDocsConfiguration
    : EntityConfiguration<ProductTechnicalDocs>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ProductTechnicalDocs> builder)
    {
        builder.ToTable(
            "ProductTechnicalDocs"
        );


        // =====================================================
        // Product
        // =====================================================

        builder.Property(
                x => x.ProductId)
            .IsRequired();


        builder.HasIndex(
                x => x.ProductId)
            .IsUnique();


        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<ProductTechnicalDocs>(
                x => x.ProductId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Ignore public read-only views
        // =====================================================

        builder.Ignore(
            x => x.ApprovedSpecifications);

        builder.Ignore(
            x => x.ApprovedCompositions);

        builder.Ignore(
            x => x.ApprovedMoaValidations);


        // =====================================================
        // Approved Specifications History
        // =====================================================

        builder.ComplexCollection<
            List<DatedApprovalDocument>,
            DatedApprovalDocument>(
            "_approvedSpecifications",
            documents =>
            {
                documents.ToJson(
                    "ApprovedSpecifications");

                documents.Property(
                    x => x.ApprovalDate);

                documents.ComplexProperty(
                    x => x.Attachment);
            });


        // =====================================================
        // Approved Composition History
        // =====================================================

        builder.ComplexCollection<
            List<DatedApprovalDocument>,
            DatedApprovalDocument>(
            "_approvedCompositions",
            documents =>
            {
                documents.ToJson(
                    "ApprovedCompositions");

                documents.Property(
                    x => x.ApprovalDate);

                documents.ComplexProperty(
                    x => x.Attachment);
            });


        // =====================================================
        // Approved MOA + Validation History
        // =====================================================

        builder.ComplexCollection<
            List<DatedApprovalDocument>,
            DatedApprovalDocument>(
            "_approvedMoaValidations",
            documents =>
            {
                documents.ToJson(
                    "ApprovedMoaValidations");

                documents.Property(
                    x => x.ApprovalDate);

                documents.ComplexProperty(
                    x => x.Attachment);
            });
    }
}