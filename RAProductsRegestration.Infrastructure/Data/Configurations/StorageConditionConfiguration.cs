using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.StorageConditions;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class StorageConditionConfiguration
    : EntityConfiguration<StorageCondition>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<StorageCondition> builder)
    {
        builder.ToTable("StorageConditions");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}