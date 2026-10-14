using MediatR;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Common.Interfaces.Messaging
{
    public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
    {
    }
}
