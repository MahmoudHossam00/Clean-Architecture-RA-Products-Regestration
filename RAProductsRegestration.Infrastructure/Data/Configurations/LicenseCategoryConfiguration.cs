using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.LicenseCategories;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class LicenseCategoryConfiguration
    : EntityConfiguration<LicenseCategory>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<LicenseCategory> builder)
    {
        builder.ToTable(
            "LicenseCategories");


        builder.Property(
                x => x.Name)
            .HasMaxLength(150)
            .IsRequired();


        builder.HasIndex(
                x => x.Name)
            .IsUnique();
    }
}