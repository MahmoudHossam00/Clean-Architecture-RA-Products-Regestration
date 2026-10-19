using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Users.Users.Update
{
    internal sealed class Handler(
    IIdentityService identityService)
    : ICommandHandler<
        UpdateUserCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            if (
                string.IsNullOrWhiteSpace(
                    request.UserName))
            {
                return Error.Validation(
                    "User.UserNameRequired",
                    "Username is required.");
            }


            if (
                string.IsNullOrWhiteSpace(
                    request.Email))
            {
                return Error.Validation(
                    "User.EmailRequired",
                    "Email is required.");
            }


            return await identityService
                .UpdateUserAsync(
                    request.UserId,
                    request.UserName,
                    request.Email,
                    request.ShortName,
                    request.EmployeeId,
                    request.IsDisabled,
                    cancellationToken);
        }
    }
}
