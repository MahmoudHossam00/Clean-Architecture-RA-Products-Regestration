using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.DosageForms;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class DosageFormConfiguration
    : EntityConfiguration<DosageForm>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<DosageForm> builder)
    {
        builder.ToTable("DosageForms");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}