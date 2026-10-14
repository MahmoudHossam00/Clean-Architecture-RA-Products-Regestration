using MediatR;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Common.Interfaces.Messaging
{
    public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>
    {
    }
}
