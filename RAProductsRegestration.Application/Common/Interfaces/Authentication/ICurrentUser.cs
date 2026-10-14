using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Authentication
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        Guid? UserId { get; }

        string? UserName { get; }

        string? Email { get; }

        IReadOnlyCollection<string> Roles { get; }

        bool IsInRole(string role);
    }
}
