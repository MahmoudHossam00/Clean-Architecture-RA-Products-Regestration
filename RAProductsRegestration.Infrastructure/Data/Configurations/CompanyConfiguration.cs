using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Companies;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class CompanyConfiguration
    : EntityConfiguration<Company>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}