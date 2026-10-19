using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Authentication.GetCurrentUser
{
    public sealed record GetCurrentUserQuery()
        : IQuery<UserResponse>;
}
