using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.Application.Common.Security;
using RAProductsRegestration.Application.Features.Users.Commands.CreateUser;

using RAProductsRegestration.Application.Features.Users.Users.GetAll;
using RAProductsRegestration.Application.Features.Users.Users.GetById;
using RAProductsRegestration.Application.Features.Users.Users.GetRoles;
using RAProductsRegestration.Application.Features.Users.Users.SetRoles;
using RAProductsRegestration.Application.Features.Users.Users.Update;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/users")]
[Authorize(
    Roles = AppRoles.Admin)]
public sealed class UsersController(
    ISender sender)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new CreateUserCommand(
                    request.UserName,
                    request.Email,
                    request.Password,
                    request.ShortName,
                    request.EmployeeId,
                    request.Roles),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            userId =>
                Ok(
                    new
                    {
                        id = userId
                    }),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new GetAllUsersQuery(),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            users =>
                Ok(users),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(
        string userId,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new GetUserByIdQuery(
                    userId),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            user =>
                Ok(user),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }


    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetRoles(
        string userId,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new GetUserRolesQuery(
                    userId),
                cancellationToken);


        return result.OnMatch<IActionResult>(
            roles =>
                Ok(roles),

            errors =>
                BadRequest(
                    new
                    {
                        errors
                    }));
    }


    [HttpPut("{userId}")]
    public async Task<IActionResult> Update(
        string userId,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new UpdateUserCommand(
                    userId,
                    request.UserName,
                    request.Email,
                    request.ShortName,
                    request.EmployeeId,
                    request.IsDisabled),
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


    [HttpPut("{userId}/roles")]
    public async Task<IActionResult> SetRoles(
        string userId,
        SetUserRolesRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new SetUserRolesCommand(
                    userId,
                    request.Roles),
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


public sealed record CreateUserRequest(
    string UserName,
    string Email,
    string Password,
    string? ShortName,
    Guid? EmployeeId,
    string[] Roles);


public sealed record UpdateUserRequest(
    string UserName,
    string Email,
    string? ShortName,
    Guid? EmployeeId,
    bool IsDisabled);


public sealed record SetUserRolesRequest(
    string[] Roles);