using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Users.Users.GetRoles
{
    internal sealed class Handler(
    IIdentityService identityService)
    : IQueryHandler<
        GetUserRolesQuery,
        string[]>
    {
        public async Task<Result<string[]>> Handle(
            GetUserRolesQuery request,
            CancellationToken cancellationToken)
        {
            var roles =
                await identityService
                    .GetUserRolesAsync(
                        request.UserId,
                        cancellationToken);


            if (roles is null)
            {
                return Error.NotFound(
                    "User.NotFound",
                    $"User '{request.UserId}' was not found.");
            }


            return roles;
        }
    }
}
