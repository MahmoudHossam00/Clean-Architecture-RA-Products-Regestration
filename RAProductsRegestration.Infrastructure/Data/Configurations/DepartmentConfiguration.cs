using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Departments;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal sealed class DepartmentConfiguration
    : EntityConfiguration<Department>
{
    protected override void ConfigureEntity(
        EntityTypeBuilder<Department> builder)
    {
        builder.ToTable(
            "Departments");


        builder.Property(
                x => x.Name)
            .HasMaxLength(150)
            .IsRequired();


        builder.HasIndex(
                x => x.Name)
            .IsUnique();
    }
}