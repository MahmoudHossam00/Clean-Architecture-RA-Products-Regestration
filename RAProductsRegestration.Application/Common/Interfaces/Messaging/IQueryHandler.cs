using MediatR;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Common.Interfaces.Messaging
{
    public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
    {
    }
}
