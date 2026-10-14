using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Products;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class ProductConfiguration
    : EntityConfiguration<Product>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(x => x.CTDId)
            .IsRequired();

        builder.Property(x => x.ProductCode)
            .IsRequired();

        builder.Property(x => x.ProductName)
            .IsRequired();

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.HasIndex(x => x.CTDId)
            .IsUnique();

        builder.HasIndex(x => x.ProductCode)
            .IsUnique();
    }
}