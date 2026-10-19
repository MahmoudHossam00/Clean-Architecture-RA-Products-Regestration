using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Users.Users.Update
{
    public sealed record UpdateUserCommand(
        string UserId,
        string UserName,
        string Email,
        string? ShortName,
        Guid? EmployeeId,
        bool IsDisabled)
        : ICommand<Updated>;
}
