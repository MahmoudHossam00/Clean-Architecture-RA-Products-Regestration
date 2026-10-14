using MediatR;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Messaging
{
    public interface ICommand<TResponse>
    : IRequest<Result<TResponse>>
    {
    }
}
