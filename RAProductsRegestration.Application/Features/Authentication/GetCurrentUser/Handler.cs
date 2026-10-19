using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Authentication.GetCurrentUser
{
    internal sealed class Handler(
    IIdentityService identityService,
    ICurrentUserService currentUserService)
    : IQueryHandler<
        GetCurrentUserQuery,
        UserResponse>
    {
        public async Task<Result<UserResponse>> Handle(
            GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            var userId =
                currentUserService.UserId;


            if (
                string.IsNullOrWhiteSpace(
                    userId))
            {
                return Error.UnAuthorized(
                    "Authentication.Unauthorized",
                    "User is not authenticated.");
            }


            var user =
                await identityService.GetByIdAsync(
                    userId,
                    cancellationToken);


            if (user is null)
            {
                return Error.NotFound(
                    "User.NotFound",
                    "Current user was not found.");
            }


            return user;
        }
    }
}
