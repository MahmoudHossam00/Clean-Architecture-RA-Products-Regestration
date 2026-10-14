using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.ProductBasicDatas
    .ProductActiveIngredients.ApprovedSuppliers;
using RAProductsRegestration.Domain.Suppliers;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class ApprovedSupplierConfiguration
    : EntityConfiguration<ApprovedSupplier>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ApprovedSupplier> builder)
    {
        builder.ToTable(
            "ApprovedSuppliers");

        builder.Property(x => x.SupplierId)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(
                x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}