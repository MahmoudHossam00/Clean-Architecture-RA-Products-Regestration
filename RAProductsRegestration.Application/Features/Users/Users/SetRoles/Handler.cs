using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Users.Users.SetRoles
{
    internal sealed class Handler(
    IIdentityService identityService)
    : ICommandHandler<
        SetUserRolesCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            SetUserRolesCommand request,
            CancellationToken cancellationToken)
        {
            if (
                request.Roles is null ||
                request.Roles.Length == 0)
            {
                return Error.Validation(
                    "User.RoleRequired",
                    "At least one role is required.");
            }


            return await identityService
                .SetUserRolesAsync(
                    request.UserId,
                    request.Roles,
                    cancellationToken);
        }
    }
}
