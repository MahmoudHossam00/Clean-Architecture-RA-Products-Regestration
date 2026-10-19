using RAProductsRegestration.Domain.Common;
using System.Linq.Expressions;

namespace RAProductsRegestration.Application.Common.Interfaces.Persistence;

public interface IRepository<TAggregate>
    where TAggregate : Entity, IAggregateRoot
{
    Task<TAggregate?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);



    Task<TAggregate?> FirstOrDefaultAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TAggregate?> SingleOrDefaultAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TAggregate, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TAggregate>> WhereAsync(
        Expression<Func<TAggregate, bool>> predicate,
        CancellationToken cancellationToken = default);

    void Add(TAggregate aggregate);

    void AddRange(IEnumerable<TAggregate> aggregates);

    void Remove(TAggregate aggregate);
}
