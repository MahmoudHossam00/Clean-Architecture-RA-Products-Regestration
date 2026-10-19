using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Users.Users.GetAll
{
    public sealed record GetAllUsersQuery()
        : IQuery<UserResponse[]>;

    internal sealed class Handler(
    IIdentityService identityService)
    : IQueryHandler<
        GetAllUsersQuery,
        UserResponse[]>
    {
        public async Task<Result<UserResponse[]>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await identityService
                .GetAllUsersAsync(
                    cancellationToken);
        }
    }
}
