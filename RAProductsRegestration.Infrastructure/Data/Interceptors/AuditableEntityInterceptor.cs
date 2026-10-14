using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RAProductsRegestration.Application.Common.Interfaces.Authentication;
using RAProductsRegestration.Domain.Common;

namespace RAProductsRegestration.Infrastructure.Data.Interceptors;

internal sealed class AuditableEntityInterceptor(
    ICurrentUser user,
    TimeProvider dateTime)
    : SaveChangesInterceptor
{
    private readonly ICurrentUser _user = user;
    private readonly TimeProvider _dateTime = dateTime;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(
            eventData,
            result);
    }

    public override ValueTask<InterceptionResult<int>>
        SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    private void UpdateEntities(
        DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var utcNow =
            _dateTime.GetUtcNow();

        var userId =
            _user.UserId?.ToString()
            ?? "SYSTEM";

        foreach (var entry in context.ChangeTracker
             .Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    {
                        entry.Property(x => x.CreatedAtUtc)
                            .CurrentValue = utcNow;

                        entry.Property(x => x.CreatedBy)
                            .CurrentValue = userId;

                        break;
                    }

                case EntityState.Modified:
                    {
                        entry.Property(x => x.LastModifiedUtc)
                            .CurrentValue = utcNow;

                        entry.Property(x => x.ModifiedBy)
                            .CurrentValue = userId;

                        break;
                    }
            }
        }
    }
}