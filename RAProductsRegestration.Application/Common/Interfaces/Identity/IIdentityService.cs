using RAProductsRegestration.Application.Features.Authentication.Common;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Identity
{
    public interface IIdentityService
    {




        Task<bool> IsInRoleAsync(
            string userId,
            string role,
            CancellationToken cancellationToken = default);


        Task<Result<string>> CreateUserAsync(
        string userName,
        string email,
        string password,
        string? shortName,
        Guid? employeeId,
        string[] roles,
        CancellationToken cancellationToken = default);


        Task<Result<AuthenticationResponse>> LoginAsync(
            string userNameOrEmail,
            string password,
            CancellationToken cancellationToken = default);


        Task<UserResponse[]> GetAllUsersAsync(
            CancellationToken cancellationToken = default);


        Task<UserResponse?> GetByIdAsync(
            string userId,
            CancellationToken cancellationToken = default);


        Task<string[]?> GetUserRolesAsync(
            string userId,
            CancellationToken cancellationToken = default);


        Task<Result<Updated>> UpdateUserAsync(
            string userId,
            string userName,
            string email,
            string? shortName,
            Guid? employeeId,
            bool isDisabled,
            CancellationToken cancellationToken = default);


        Task<Result<Updated>> SetUserRolesAsync(
            string userId,
            string[] roles,
            CancellationToken cancellationToken = default);


        Task<Result<Updated>> ChangePasswordAsync(
            string userId,
            string currentPassword,
            string newPassword,
            CancellationToken cancellationToken = default);





        Task<Result<Updated>> CompleteFirstLoginAsync(
            string userId,
            string currentPassword,
            string newPassword,
            CancellationToken cancellationToken = default);
    }
    public sealed record UserIdentityResponse(
    string Id,
    string UserName,
    string? Email,
    string? ShortName,
    string? Picture,
    Guid? EmployeeId,
    bool IsDisabled,
    DateTimeOffset? LastLoginAtUtc);

    public sealed record UserResponse(
    string Id,
    string UserName,
    string? Email,
    string? ShortName,
    string? Picture,
    Guid? EmployeeId,
    bool IsDisabled,
    bool ChangedHisOwnPassword,
    bool ChangedHisOwnNickName,
    bool RequiresFirstLoginSetup,
    DateTimeOffset? LastLoginAtUtc,
    string[] Roles);
}
