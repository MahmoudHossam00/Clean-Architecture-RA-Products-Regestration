using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Features
    .Authentication.ChangeMyPassword;
using RAProductsRegestration.Application.Features
    .Authentication.CompleteFirstLogin;
using RAProductsRegestration.Application.Features
    .Authentication.GetCurrentUser;
using RAProductsRegestration.Application.Features
    .Authentication.Login;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/auth")]
[Authorize]
public sealed class AuthController(
    ISender sender)
    : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new LoginCommand(
                    request.UserNameOrEmail,
                    request.Password),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            success =>
                Ok(success),

            errors =>
                Unauthorized(
                    new
                    {
                        errors =
                            errors.Select(
                                x =>
                                    new
                                    {
                                        x.Code,
                                        x.Description
                                    })
                    }));
    }


    [HttpGet("me")]
    public async Task<IActionResult> Me(
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new GetCurrentUserQuery(),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            success =>
                Ok(success),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }


    [HttpPut("complete-first-login")]
    public async Task<IActionResult> CompleteFirstLogin(
        CompleteFirstLoginRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new CompleteFirstLoginCommand(
                    request.CurrentPassword,
                    request.NewPassword,
                    request.ConfirmNewPassword),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            _ =>
                NoContent(),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }


    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new ChangeMyPasswordCommand(
                    request.CurrentPassword,
                    request.NewPassword,
                    request.ConfirmNewPassword),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            _ =>
                NoContent(),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }
}


public sealed record LoginRequest(
    string UserNameOrEmail,
    string Password);


public sealed record CompleteFirstLoginRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword);


public sealed record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword);