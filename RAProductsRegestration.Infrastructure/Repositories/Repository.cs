using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Infrastructure.Data;
using System.Linq.Expressions;

namespace RAProductsRegestration.Infrastructure.Repositories;

internal class Repository<TAggregate>(
    ApplicationDbContext context)
    : IRepository<TAggregate>
    where TAggregate : Entity, IAggregateRoot
{
    private readonly DbSet<TAggregate> _dbSet =
        context.Set<TAggregate>();

    public async Task<TAggregate?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<TAggregate?> FirstOrDefaultAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(
                predicate,
                cancellationToken);
    }

    public async Task<TAggregate?> SingleOrDefaultAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .SingleOrDefaultAsync(
                predicate,
                cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(
                predicate,
                cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<TAggregate, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        if (predicate is null)
        {
            return await _dbSet.CountAsync(
                cancellationToken);
        }

        return await _dbSet.CountAsync(
            predicate,
            cancellationToken);
    }

    public async Task<IReadOnlyList<TAggregate>> WhereAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public void Add(
        TAggregate aggregate)
    {
        _dbSet.Add(aggregate);
    }

    public void AddRange(
        IEnumerable<TAggregate> aggregates)
    {
        _dbSet.AddRange(aggregates);
    }

    public void Remove(
        TAggregate aggregate)
    {
        _dbSet.Remove(aggregate);
    }
}