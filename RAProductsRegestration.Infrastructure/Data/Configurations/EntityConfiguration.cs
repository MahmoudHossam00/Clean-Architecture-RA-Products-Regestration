using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAProductsRegestration.Domain.Common;

namespace RAProductsRegestration.Infrastructure.Data.Configurations;

internal abstract class EntityConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public void Configure(
        EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        //builder.Property(x => x.Id)
        //    .ValueGeneratedNever();

        // Domain events are not persisted.
        builder.Ignore(x => x.DomainEvents);

        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(
        EntityTypeBuilder<TEntity> builder);
}