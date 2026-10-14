using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.ProductBasicDatas.ProductActiveIngredients;
using RAProductsRegestration.Domain.StorageConditions;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class ProductActiveIngredientConfiguration
    : EntityConfiguration<ProductActiveIngredient>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ProductActiveIngredient> builder)
    {
        builder.ToTable(
            "ProductActiveIngredients");

        builder.Property(x => x.ActiveIngredientId)
            .IsRequired();

        builder.Property(x => x.Strength)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PhysicalDescription)
            .HasMaxLength(2000);

        // =====================================================
        // Storage Condition
        // =====================================================

        builder.HasOne<StorageCondition>()
            .WithMany()
            .HasForeignKey(
                x => x.StorageConditionId)
            .OnDelete(DeleteBehavior.Restrict);

        // =====================================================
        // Approved Suppliers
        // =====================================================

        builder.HasMany(
                x => x.ApprovedSuppliers)
            .WithOne()
            .HasForeignKey(
                "ProductActiveIngredientId")
            .OnDelete(DeleteBehavior.Cascade);

        // =====================================================
        // Approved Packs
        // =====================================================

        builder.HasMany(
                x => x.ApprovedPacks)
            .WithOne()
            .HasForeignKey(
                "ProductActiveIngredientId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.ComplexProperty(
    x => x.ShelfLife,
    shelfLife =>
    {
        shelfLife.ToJson();

        shelfLife.Property(
            x => x.NumberOfMonths
        );

        shelfLife.Property(
            x => x.ApprovalDate
        );

        shelfLife.ComplexCollection(
            x => x.Attachments
        );
    }

);
    }
}