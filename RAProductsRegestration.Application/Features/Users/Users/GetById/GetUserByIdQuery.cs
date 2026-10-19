using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Users.Users.GetById
{
    public sealed record GetUserByIdQuery(
        string UserId)
        : IQuery<UserResponse>;
    internal sealed class Handler(
    IIdentityService identityService)
    : IQueryHandler<
        GetUserByIdQuery,
        UserResponse>
    {
        public async Task<Result<UserResponse>> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user =
                await identityService.GetByIdAsync(
                    request.UserId,
                    cancellationToken);


            if (user is null)
            {
                return Error.NotFound(
                    "User.NotFound",
                    $"User '{request.UserId}' was not found.");
            }


            return user;
        }
    }
}
