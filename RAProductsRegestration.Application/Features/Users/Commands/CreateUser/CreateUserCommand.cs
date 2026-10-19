using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand(
        string UserName,
        string Email,
        string Password,
        string? ShortName,
        Guid? EmployeeId,
        string[] Roles)
        : ICommand<string>;

    internal sealed class Handler(
    IIdentityService identityService)
    : ICommandHandler<
        CreateUserCommand,
        string>
    {
        public async Task<Result<string>> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            return await identityService
                .CreateUserAsync(
                    request.UserName,
                    request.Email,
                    request.Password,
                    request.ShortName,
                    request.EmployeeId,
                    request.Roles,
                    cancellationToken);
        }
    }
}
