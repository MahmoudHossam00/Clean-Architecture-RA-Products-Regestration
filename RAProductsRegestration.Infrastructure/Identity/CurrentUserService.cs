using Microsoft.AspNetCore.Http;
using RAProductsRegestration.Application.Common.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Identity
{
    internal sealed class CurrentUserService(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
    {
        public string? UserId =>
            httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(
                    ClaimTypes.NameIdentifier);


        public bool IsAuthenticated =>
            httpContextAccessor
                .HttpContext?
                .User
                .Identity?
                .IsAuthenticated
            ?? false;
    }
}
