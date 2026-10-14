using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.MarketingTypes;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class MarketingTypeConfiguration
    : EntityConfiguration<MarketingType>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<MarketingType> builder)
    {
        builder.ToTable(
            "MarketingTypes");


        builder.Property(
                x => x.Name)
            .HasMaxLength(150)
            .IsRequired();


        builder.HasIndex(
                x => x.Name)
            .IsUnique();
    }
}