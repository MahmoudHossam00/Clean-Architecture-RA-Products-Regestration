using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.ActiveIngredients;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class ActiveIngredientConfiguration
    : EntityConfiguration<ActiveIngredient>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<ActiveIngredient> builder)
    {
        builder.ToTable("ActiveIngredients");

        builder.Property(x => x.CTDId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.HasIndex(x => x.CTDId)
            .IsUnique();

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}