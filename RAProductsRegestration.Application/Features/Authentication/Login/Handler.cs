using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Features.Authentication.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Features.Authentication.Login
{
    internal sealed class Handler(
    IIdentityService identityService)
    : ICommandHandler<
        LoginCommand,
        AuthenticationResponse>
    {
        public async Task<Result<AuthenticationResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            if (
                string.IsNullOrWhiteSpace(
                    request.UserNameOrEmail) ||
                string.IsNullOrWhiteSpace(
                    request.Password))
            {
                return Error.Validation(
                    "Authentication.CredentialsRequired",
                    "Username/email and password are required.");
            }


            return await identityService
                .LoginAsync(
                    request.UserNameOrEmail.Trim(),
                    request.Password,
                    cancellationToken);
        }
    }
}
