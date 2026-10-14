using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.DosageForms;
using RAProductsRegestration.Domain.ProductBasicDatas;
using RAProductsRegestration.Domain.ProductBasicDatas.ShelfLives;
using RAProductsRegestration.Domain.Products;
using RAProductsRegestration.Domain.StorageConditions;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class ProductBasicDataConfiguration
    : EntityConfiguration<ProductBasicData>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ProductBasicData> builder)
    {
        builder.ToTable(
            "ProductBasicData");


        // =====================================================
        // Computed / Convenience Properties
        // =====================================================

        /*
         * These properties are calculated from history
         * collections and must NOT be mapped by EF.
         */

        builder.Ignore(
            x => x.CurrentPV);

        builder.Ignore(
            x => x.CurrentDecree600);

        builder.Ignore(
            x => x.CurrentShelfLife);

        builder.Ignore(
            x => x.CurrentShelfLifeHistory);


        // =====================================================
        // Basic Properties
        // =====================================================

        builder.Property(
                x => x.ProductId)
            .IsRequired();


        builder.Property(
                x => x.TradeName)
            .HasMaxLength(
                250);


        builder.Property(
            x => x.DosageFormId);


        builder.Property(
            x => x.StorageConditionId);


        builder.Property(
                x => x.PhysicalDescription)
            .HasMaxLength(
                2000);


        // One Product can only have one ProductBasicData
        builder.HasIndex(
                x => x.ProductId)
            .IsUnique();


        // =====================================================
        // Product
        // =====================================================

        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<ProductBasicData>(
                x => x.ProductId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Dosage Form
        // =====================================================

        builder.HasOne<DosageForm>()
            .WithMany()
            .HasForeignKey(
                x => x.DosageFormId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Storage Condition
        // =====================================================

        builder.HasOne<StorageCondition>()
            .WithMany()
            .HasForeignKey(
                x => x.StorageConditionId)
            .OnDelete(
                DeleteBehavior.Restrict);


        // =====================================================
        // Shelf Life History
        // =====================================================

        builder
            .HasMany(
                x => x.ShelfLifeHistory)
            .WithOne()
            .HasForeignKey(
                "ProductBasicDataId")
            .OnDelete(
                DeleteBehavior.Cascade);


        builder.Navigation(
                x => x.ShelfLifeHistory)
            .HasField(
                "_shelfLifeHistory")
            .UsePropertyAccessMode(
                PropertyAccessMode.Field)
            .AutoInclude();


        // =====================================================
        // PV History
        // =====================================================

        builder
            .HasMany(
                x => x.PVHistory)
            .WithOne()
            .HasForeignKey(
                "ProductBasicDataId")
            .OnDelete(
                DeleteBehavior.Cascade);


        builder.Navigation(
                x => x.PVHistory)
            .HasField(
                "_pvHistory")
            .UsePropertyAccessMode(
                PropertyAccessMode.Field)
            .AutoInclude();


        // =====================================================
        // Decree 600 History
        // =====================================================

        builder
            .HasMany(
                x => x.Decree600History)
            .WithOne()
            .HasForeignKey(
                "ProductBasicDataId")
            .OnDelete(
                DeleteBehavior.Cascade);


        builder.Navigation(
                x => x.Decree600History)
            .HasField(
                "_decree600History")
            .UsePropertyAccessMode(
                PropertyAccessMode.Field)
            .AutoInclude();


        // =====================================================
        // Active Ingredients
        // =====================================================

        builder
            .HasMany(
                x => x.ProductActiveIngredients)
            .WithOne()
            .HasForeignKey(
                "ProductBasicDataId")
            .OnDelete(
                DeleteBehavior.Cascade);


        // =====================================================
        // Product Approved Packs
        // =====================================================

        builder
            .HasMany(
                x => x.ApprovedPacks)
            .WithOne()
            .HasForeignKey(
                "ProductBasicDataId")
            .OnDelete(
                DeleteBehavior.Cascade);


        // =====================================================
        // Approved Prices
        // =====================================================

        builder
            .HasMany(
                x => x.ApprovedPrices)
            .WithOne()
            .HasForeignKey(
                "ProductBasicDataId")
            .OnDelete(
                DeleteBehavior.Cascade);


        // =====================================================
        // Approved Manufacturers
        // =====================================================

        builder
            .HasMany(
                x => x.ApprovedManufacturers)
            .WithOne()
            .HasForeignKey(
                "ProductBasicDataId")
            .OnDelete(
                DeleteBehavior.Cascade);
    }



internal sealed class ShelfLifeHistoryItemConfiguration
    : EntityConfiguration<ShelfLifeHistoryItem>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ShelfLifeHistoryItem> builder)
    {
        builder.ToTable(
            "ProductBasicDataShelfLifeHistory");


        // =====================================================
        // Version
        // =====================================================

        builder.Property(
                x => x.VersionNumber)
            .IsRequired();


        /*
         * A ProductBasicData cannot contain
         * duplicate shelf-life version numbers.
         */
        builder.HasIndex(
                "ProductBasicDataId",
                nameof(
                    ShelfLifeHistoryItem.VersionNumber))
            .IsUnique();


        // =====================================================
        // Shelf Life Snapshot
        // =====================================================

        builder.ComplexProperty(
            x => x.ShelfLife,
            shelfLife =>
            {
                /*
                 * Store the complete historical snapshot
                 * as JSON in the history row.
                 */
                shelfLife.ToJson();


                shelfLife.Property(
                    x => x.NumberOfMonths);


                shelfLife.Property(
                    x => x.ApprovalDate);


                shelfLife.ComplexCollection(
                    x => x.Attachments);
            });
    }
}
}