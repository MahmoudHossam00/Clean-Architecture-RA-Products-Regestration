using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record AuthenticatedUser(
    Guid UserId,
    string UserName,
    string? Email,
    IReadOnlyCollection<string> Roles);
}
