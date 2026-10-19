using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Identity;
using RAProductsRegestration.Application.Common.Security;
using RAProductsRegestration.Application.Features.Authentication.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Infrastructure.Identity;

internal sealed class IdentityService(
    UserManager<AppUser> userManager,
    JwtTokenProvider jwtTokenProvider)
    : IIdentityService
{
    public async Task<UserResponse?> GetByIdAsync(
    string userId,
    CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == userId,
                    cancellationToken);


        if (user is null)
        {
            return null;
        }


        var roles =
            (await userManager
                .GetRolesAsync(
                    user))
            .ToArray();


        return MapUser(
            user,
            roles);
    }

    public async Task<Result<Updated>> UpdateUserAsync(
    string userId,
    string userName,
    string email,
    string? shortName,
    Guid? employeeId,
    bool isDisabled,
    CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.FindByIdAsync(
                userId);


        if (user is null)
        {
            return Error.NotFound(
                "User.NotFound",
                $"User '{userId}' was not found.");
        }


        var usernameOwner =
            await userManager.FindByNameAsync(
                userName);


        if (
            usernameOwner is not null &&
            usernameOwner.Id != userId)
        {
            return Error.Conflict(
                "User.UserNameAlreadyExists",
                "Username already exists.");
        }


        var emailOwner =
            await userManager.FindByEmailAsync(
                email);


        if (
            emailOwner is not null &&
            emailOwner.Id != userId)
        {
            return Error.Conflict(
                "User.EmailAlreadyExists",
                "Email already exists.");
        }


        if (
            !string.Equals(
                user.UserName,
                userName,
                StringComparison.Ordinal))
        {
            var userNameResult =
                await userManager.SetUserNameAsync(
                    user,
                    userName.Trim());


            if (!userNameResult.Succeeded)
            {
                return IdentityFailure(
                    "User.UpdateUserNameFailed",
                    userNameResult);
            }
        }


        if (
            !string.Equals(
                user.Email,
                email,
                StringComparison.OrdinalIgnoreCase))
        {
            var emailResult =
                await userManager.SetEmailAsync(
                    user,
                    email.Trim());


            if (!emailResult.Succeeded)
            {
                return IdentityFailure(
                    "User.UpdateEmailFailed",
                    emailResult);
            }
        }


        user.ShortName =
            string.IsNullOrWhiteSpace(
                shortName)
                ? null
                : shortName.Trim();


        user.EmployeeId =
            employeeId;


        user.IsDisabled =
            isDisabled;


        var updateResult =
            await userManager.UpdateAsync(
                user);


        if (!updateResult.Succeeded)
        {
            return IdentityFailure(
                "User.UpdateFailed",
                updateResult);
        }


        return Result.Updated;
    }


    private static Error IdentityFailure(
    string code,
    IdentityResult result)
    {
        return Error.Validation(
            code,
            string.Join(
                " | ",
                result.Errors
                    .Select(x =>
                        x.Description)));
    }


    public async Task<Result<Updated>> ChangePasswordAsync(
    string userId,
    string currentPassword,
    string newPassword,
    CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.FindByIdAsync(
                userId);


        if (user is null)
        {
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");
        }


        var result =
            await userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword);


        if (!result.Succeeded)
        {
            return IdentityFailure(
                "User.ChangePasswordFailed",
                result);
        }


        user.ChangedHisOwnPassword =
            true;


        var updateResult =
            await userManager.UpdateAsync(
                user);


        if (!updateResult.Succeeded)
        {
            return IdentityFailure(
                "User.UpdateFailed",
                updateResult);
        }


        return Result.Updated;
    }
    private static UserResponse MapUser(
    AppUser user,
    string[] roles)
    {
        return new UserResponse(
            user.Id,
            user.UserName!,
            user.Email,
            user.ShortName,
            user.Picture,
            user.EmployeeId,
            user.IsDisabled,
            user.ChangedHisOwnPassword,
            user.ChangedHisOwnNickName,

                !user.ChangedHisOwnPassword,

            user.LastLoginAtUtc,
            roles);
    }


    public async Task<bool> IsInRoleAsync(
        string userId,
        string role,
        CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.FindByIdAsync(
                userId.ToString());

        if (user is null)
        {
            return false;
        }

        return await userManager
            .IsInRoleAsync(
                user,
                role);
    }


    // =========================================================
    // Create User
    // =========================================================

    public async Task<Result<string>> CreateUserAsync(
        string userName,
        string email,
        string password,
        string? shortName,
        Guid? employeeId,
        string[] roles,
        CancellationToken cancellationToken = default)
    {
        var existingByName =
            await userManager.FindByNameAsync(
                userName);


        if (existingByName is not null)
        {
            return Error.Conflict(
                "User.UserNameAlreadyExists",
                "Username already exists.");
        }


        var existingByEmail =
            await userManager.FindByEmailAsync(
                email);


        if (existingByEmail is not null)
        {
            return Error.Conflict(
                "User.EmailAlreadyExists",
                "Email already exists.");
        }


        var normalizedRoles =
            roles
                .Select(x =>
                    x.Trim()
                        .ToUpperInvariant())
                .Distinct()
                .ToArray();


        var invalidRoles =
            normalizedRoles
                .Except(
                    AppRoles.All,
                    StringComparer.OrdinalIgnoreCase)
                .ToArray();


        if (
            invalidRoles.Length > 0
        )
        {
            return Error.Validation(
                "User.InvalidRole",
                $"Invalid role(s): " +
                $"{string.Join(", ", invalidRoles)}");
        }


        var user =
            new AppUser
            {
                UserName =
                    userName.Trim(),

                Email =
                    email.Trim(),

                ShortName =
                    string.IsNullOrWhiteSpace(
                        shortName)
                        ? null
                        : shortName.Trim(),

                EmployeeId =
                    employeeId,

                IsDisabled =
                    false
            };


        var createResult =
            await userManager.CreateAsync(
                user,
                password);


        if (
            !createResult.Succeeded
        )
        {
            var errors =
                string.Join(
                    " | ",
                    createResult.Errors
                        .Select(x =>
                            x.Description));


            return Error.Validation(
                "User.CreateFailed",
                errors);
        }


        if (
            normalizedRoles.Length > 0
        )
        {
            var roleResult =
                await userManager.AddToRolesAsync(
                    user,
                    normalizedRoles);


            if (
                !roleResult.Succeeded
            )
            {
                await userManager.DeleteAsync(
                    user);


                var errors =
                    string.Join(
                        " | ",
                        roleResult.Errors
                            .Select(x =>
                                x.Description));


                return Error.Validation(
                    "User.RoleAssignmentFailed",
                    errors);
            }
        }


        return user.Id;
    }

    public async Task<UserResponse[]> GetAllUsersAsync(
    CancellationToken cancellationToken = default)
    {
        var users =
            await userManager.Users
                .AsNoTracking()
                .OrderBy(x =>
                    x.UserName)
                .ToArrayAsync(
                    cancellationToken);


        var result =
            new List<UserResponse>(
                users.Length);


        foreach (
            var user
            in users)
        {
            var roles =
                (await userManager
                    .GetRolesAsync(
                        user))
                .ToArray();


            result.Add(
                MapUser(
                    user,
                    roles));
        }


        return result.ToArray();
    }
    // =========================================================
    // Login
    // =========================================================

    public async Task<Result<AuthenticationResponse>>
        LoginAsync(
            string userNameOrEmail,
            string password,
            CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.Users
                .FirstOrDefaultAsync(
                    x =>
                        x.UserName ==
                            userNameOrEmail ||

                        x.Email ==
                            userNameOrEmail,
                    cancellationToken);


        if (
            user is null
        )
        {
            return Error.UnAuthorized(
                "Authentication.InvalidCredentials",
                "Invalid username/email or password.");
        }


        if (
            user.IsDisabled
        )
        {
            return Error.Forbidden(
                "Authentication.UserDisabled",
                "This user account is disabled.");
        }


        var validPassword =
            await userManager.CheckPasswordAsync(
                user,
                password);


        if (
            !validPassword
        )
        {
            return Error.UnAuthorized(
                "Authentication.InvalidCredentials",
                "Invalid username/email or password.");
        }


        var roles =
    (await userManager
        .GetRolesAsync(
            user))
    .ToArray();


        var (
            token,
            expiresAtUtc) =
            jwtTokenProvider.Generate(
                user,
                roles);


        user.LastLoginAtUtc =
            DateTimeOffset.UtcNow;


        await userManager.UpdateAsync(
            user);


        return new AuthenticationResponse(
            token,
            expiresAtUtc,
            user.Id,
            user.UserName!,
            user.Email,
            user.ShortName,
            user.Picture,
            user.EmployeeId,
            roles,
            user.ChangedHisOwnPassword,
            !user.ChangedHisOwnPassword);
    }

    // =========================================================
    // Get User Roles
    // =========================================================

    public async Task<string[]> GetUserRolesAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.FindByIdAsync(
                userId);

        if (user is null)
        {
            return null;
        }


        return (
            await userManager
                .GetRolesAsync(
                    user))
            .ToArray();
    }
    public async Task<Result<Updated>>
    CompleteFirstLoginAsync(
        string userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.FindByIdAsync(
                userId);


        if (user is null)
        {
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");
        }


        if (user.IsDisabled)
        {
            return Error.Forbidden(
                "Authentication.UserDisabled",
                "This user account is disabled.");
        }


        if (user.ChangedHisOwnPassword)
        {
            return Error.Conflict(
                "Authentication.FirstLoginAlreadyCompleted",
                "First login setup has already been completed.");
        }


        var passwordResult =
            await userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword);


        if (!passwordResult.Succeeded)
        {
            return IdentityFailure(
                "User.ChangePasswordFailed",
                passwordResult);
        }


        user.ChangedHisOwnPassword =
            true;


        var updateResult =
            await userManager.UpdateAsync(
                user);


        if (!updateResult.Succeeded)
        {
            return IdentityFailure(
                "User.FirstLoginUpdateFailed",
                updateResult);
        }


        return Result.Updated;
    }

    // =========================================================
    // Replace User Roles
    // =========================================================

    public async Task<Result<Updated>> SetUserRolesAsync(
        string userId,
        string[] roles,
        CancellationToken cancellationToken = default)
    {
        var user =
            await userManager.FindByIdAsync(
                userId);


        if (
            user is null
        )
        {
            return Error.NotFound(
                "User.NotFound",
                $"User '{userId}' was not found.");
        }


        var normalizedRoles =
            roles
                .Select(x =>
                    x.Trim()
                        .ToUpperInvariant())
                .Distinct()
                .ToArray();


        var invalidRoles =
            normalizedRoles
                .Except(
                    AppRoles.All,
                    StringComparer.OrdinalIgnoreCase)
                .ToArray();


        if (
            invalidRoles.Length > 0
        )
        {
            return Error.Validation(
                "User.InvalidRole",
                $"Invalid role(s): " +
                $"{string.Join(", ", invalidRoles)}");
        }


        var currentRoles =
            await userManager
                .GetRolesAsync(
                    user);


        var removeResult =
            await userManager
                .RemoveFromRolesAsync(
                    user,
                    currentRoles);


        if (
            !removeResult.Succeeded
        )
        {
            return Error.Validation(
                "User.RemoveRolesFailed",
                string.Join(
                    " | ",
                    removeResult.Errors
                        .Select(x =>
                            x.Description)));
        }


        if (
            normalizedRoles.Length > 0
        )
        {
            var addResult =
                await userManager
                    .AddToRolesAsync(
                        user,
                        normalizedRoles);


            if (
                !addResult.Succeeded
            )
            {
                return Error.Validation(
                    "User.AddRolesFailed",
                    string.Join(
                        " | ",
                        addResult.Errors
                            .Select(x =>
                                x.Description)));
            }
        }


        return Result.Updated;
    }
}