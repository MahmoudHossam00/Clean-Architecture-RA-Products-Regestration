using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Authentication
{
    public interface ITokenProvider
    {
        AccessTokenResult GenerateAccessToken(
            AuthenticatedUser user);

        string GenerateRefreshToken();
    }
}
