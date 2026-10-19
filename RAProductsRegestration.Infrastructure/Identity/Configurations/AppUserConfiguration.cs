using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RAProductsRegestration.Infrastructure.Identity.Configurations;

internal sealed class AppUserConfiguration
    : IEntityTypeConfiguration<AppUser>
{
    public void Configure(
        EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(
                x => x.ShortName)
            .HasMaxLength(150);


        builder.Property(
                x => x.Picture)
            .HasMaxLength(1000);


        builder.Property(
                x => x.IsDisabled)
            .HasDefaultValue(false);


        builder.Property(
                x => x.ChangedHisOwnPassword)
            .HasDefaultValue(false);


        builder.Property(
                x => x.ChangedHisOwnNickName)
            .HasDefaultValue(false);


        builder.Property(
            x => x.EmployeeId);


        builder.Property(
            x => x.LastLoginAtUtc);
    }
}