using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Products;
using RAProductsRegestration.Domain.Variations;
using RAProductsRegestration.Infrastructure.Data.Configurations;

namespace RAProductsRegestration.Infrastructure.Configurations;

internal sealed class VariationConfiguration
    : EntityConfiguration<Variation>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Variation> builder)
    {
        builder.ToTable(
            "Variations");


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
        // Cause
        // =====================================================

        builder.Property(
                x => x.Cause)
            .HasMaxLength(2000)
            .IsRequired();


        // =====================================================
        // Deadline
        // =====================================================

        builder.Property(
                x => x.Deadline)
            .IsRequired();


        builder.Property(
                x => x.DeadlineNote)
            .HasMaxLength(2000);


        // =====================================================
        // Approval
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
                "VariationId")
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