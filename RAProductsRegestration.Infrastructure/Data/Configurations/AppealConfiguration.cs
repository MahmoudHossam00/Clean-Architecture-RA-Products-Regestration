using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Products;
using RAProductsRegestration.Infrastructure.Data.Configurations;

namespace RAProductsRegestration.Infrastructure.Configurations;

internal sealed class AppealConfiguration
    : EntityConfiguration<Appeal>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Appeal> builder)
    {
        builder.ToTable(
            "Appeals");


        // =====================================================
        // Product
        // =====================================================

        builder.Property(
                x => x.ProductId)
            .IsRequired();


        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(
                x => x.ProductId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Initiator
        // =====================================================

        builder.Property(
                x => x.Initiator)
            .HasMaxLength(250)
            .IsRequired();


        // =====================================================
        // Appeal Type
        // =====================================================

        builder.Property(
                x => x.AppealTypeId)
            .IsRequired();


        builder.HasOne<AppealType>()
            .WithMany()
            .HasForeignKey(
                x => x.AppealTypeId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Appeal Date
        // =====================================================

        builder.Property(
                x => x.AppealDate)
            .IsRequired();


        // =====================================================
        // Notes
        // =====================================================

        builder.Property(
                x => x.Notes)
            .HasMaxLength(4000);


        // =====================================================
        // Appeal Approval
        // =====================================================

        builder.ComplexProperty(
            x => x.Approval,
            approval =>
            {
                approval.ToJson(
                    "Approval");


                approval.ComplexProperty(
                    x => x.Attachment);
            });


        // =====================================================
        // Requirements
        // =====================================================

        builder.HasMany(
                x => x.Requirements)
            .WithOne()
            .HasForeignKey(
                "AppealId")
            .OnDelete(
                DeleteBehavior.Cascade);


        builder.Navigation(
                x => x.Requirements)
            .HasField(
                "_requirements")
            .UsePropertyAccessMode(
                PropertyAccessMode.Field);
    }
}