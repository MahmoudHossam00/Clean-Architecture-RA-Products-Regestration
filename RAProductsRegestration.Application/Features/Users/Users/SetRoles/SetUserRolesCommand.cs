using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Users.Users.SetRoles
{
    public sealed record SetUserRolesCommand(
        string UserId,
        string[] Roles)
        : ICommand<Updated>;
}
