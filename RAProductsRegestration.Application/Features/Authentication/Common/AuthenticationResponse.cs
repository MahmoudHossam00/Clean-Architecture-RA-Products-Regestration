using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Authentication.Common
{
    public sealed record AuthenticationResponse(
        string Token,
        DateTimeOffset ExpiresAtUtc,
        string UserId,
        string UserName,
        string? Email,
        string? ShortName,
        string? Picture,
        Guid? EmployeeId,
        string[] Roles,
        bool ChangedHisOwnPassword,
        bool RequiresFirstLoginSetup);
}
