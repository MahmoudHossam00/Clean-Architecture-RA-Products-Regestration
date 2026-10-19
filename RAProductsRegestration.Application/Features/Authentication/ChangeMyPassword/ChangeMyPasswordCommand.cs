using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Authentication.ChangeMyPassword
{
    public sealed record ChangeMyPasswordCommand(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword)
        : ICommand<Updated>;

    internal sealed class Handler(
        IIdentityService identityService,
        ICurrentUserService currentUserService)
        : ICommandHandler<
            ChangeMyPasswordCommand,
            Updated>
    {
        public async Task<Result<Updated>> Handle(
            ChangeMyPasswordCommand request,
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


            if (
                request.NewPassword !=
                request.ConfirmNewPassword)
            {
                return Error.Validation(
                    "Authentication.PasswordMismatch",
                    "New password and confirmation do not match.");
            }


            if (
                request.CurrentPassword ==
                request.NewPassword)
            {
                return Error.Validation(
                    "Authentication.PasswordNotChanged",
                    "New password must be different from the current password.");
            }


            return await identityService
                .ChangePasswordAsync(
                    userId,
                    request.CurrentPassword,
                    request.NewPassword,
                    cancellationToken);
        }
    }
}
