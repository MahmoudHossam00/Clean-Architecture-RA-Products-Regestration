using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.LicenseCategories;
using RAProductsRegestration.Domain.MarketingTypes;
using RAProductsRegestration.Domain.ProductLicenses;
using RAProductsRegestration.Domain.Products;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class ProductLicenseConfiguration
    : EntityConfiguration<ProductLicense>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ProductLicense> builder)
    {
        builder.ToTable(
            "ProductLicenses");
        builder.HasMany(
        x => x.Requirements)
    .WithOne()
    .HasForeignKey(
        "ProductLicenseId")
    .OnDelete(
        DeleteBehavior.Cascade);


        builder.Navigation(
                x => x.Requirements)
            .HasField(
                "_requirements")
            .UsePropertyAccessMode(
                PropertyAccessMode.Field);

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
            .HasForeignKey<ProductLicense>(
                x => x.ProductId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // License Category
        // =====================================================

        builder.Property(
                x => x.LicenseCategoryId)
            .IsRequired();


        builder.HasOne<LicenseCategory>()
            .WithMany()
            .HasForeignKey(
                x => x.LicenseCategoryId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Marketing Type
        // =====================================================

        builder.Property(
                x => x.MarketingTypeId)
            .IsRequired();


        builder.HasOne<MarketingType>()
            .WithMany()
            .HasForeignKey(
                x => x.MarketingTypeId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Dates
        // =====================================================

        builder.Property(
                x => x.IssueDate)
            .IsRequired();


        builder.Property(
                x => x.ValidityDate)
            .IsRequired();


        // =====================================================
        // Registration Number / Year
        // =====================================================

        builder.Property(
                x => x.RegistrationNumberYear)
            .HasMaxLength(100)
            .IsRequired();


        // =====================================================
        // Last Approved Leaflet
        // =====================================================

        builder.ComplexProperty(
            x => x.LastApprovedLeaflet,
            approval =>
            {
                approval.ToJson();

                approval.Property(
                    x => x.Date);

                approval.ComplexProperty(
                    x => x.Attachment);
            });


        // =====================================================
        // Last Approved Layout
        // =====================================================

        builder.ComplexProperty(
            x => x.LastApprovedLayout,
            approval =>
            {
                approval.ToJson();

                approval.Property(
                    x => x.Date);

                approval.ComplexProperty(
                    x => x.Attachment);
            });
    }
}