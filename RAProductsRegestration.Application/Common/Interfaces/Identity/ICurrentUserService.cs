using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Identity
{
    public interface ICurrentUserService
    {
        string? UserId { get; }

        bool IsAuthenticated { get; }
    }
}
