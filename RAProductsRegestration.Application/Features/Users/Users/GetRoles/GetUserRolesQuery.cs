using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Users.Users.GetRoles
{
    public sealed record GetUserRolesQuery(
        string UserId)
        : IQuery<string[]>;
}
