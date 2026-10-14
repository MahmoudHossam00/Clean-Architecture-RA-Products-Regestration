using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Infrastructure.Data.Configurations;

namespace RAProductsRegestration.Infrastructure.Configurations;

internal sealed class AppealTypeConfiguration
    : EntityConfiguration<AppealType>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<AppealType> builder)
    {
        builder.ToTable(
            "AppealTypes");


        builder.Property(
                x => x.Name)
            .HasMaxLength(150)
            .IsRequired();


        builder.HasIndex(
                x => x.Name)
            .IsUnique();
    }
}
