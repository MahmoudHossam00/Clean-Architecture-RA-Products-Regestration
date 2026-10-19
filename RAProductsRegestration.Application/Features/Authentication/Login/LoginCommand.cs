using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Features.Authentication.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Authentication.Login
{
    public sealed record LoginCommand(
        string UserNameOrEmail,
        string Password)
        : ICommand<AuthenticationResponse>;
}
