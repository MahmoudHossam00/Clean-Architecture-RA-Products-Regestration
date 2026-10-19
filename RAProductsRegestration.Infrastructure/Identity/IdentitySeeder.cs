using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RAProductsRegestration.Application.Common.Security;

namespace RAProductsRegestration.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedIdentityAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        await using var scope =
            serviceProvider.CreateAsyncScope();


        var roleManager =
            scope.ServiceProvider
                .GetRequiredService<
                    RoleManager<IdentityRole>>();


        var userManager =
            scope.ServiceProvider
                .GetRequiredService<
                    UserManager<AppUser>>();


        var settings =
            scope.ServiceProvider
                .GetRequiredService<
                    IOptions<IdentitySeedSettings>>()
                .Value;


        // =====================================================
        // Seed Roles
        // =====================================================

        foreach (
            var roleName
            in AppRoles.All)
        {
            var exists =
                await roleManager
                    .RoleExistsAsync(
                        roleName);


            if (exists)
            {
                continue;
            }


            var result =
                await roleManager.CreateAsync(
                    new IdentityRole(
                        roleName));


            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        " | ",
                        result.Errors
                            .Select(
                                x =>
                                    x.Description));


                throw new InvalidOperationException(
                    $"Unable to create role " +
                    $"'{roleName}'. {errors}");
            }
        }


        // =====================================================
        // Validate Admin Settings
        // =====================================================

        if (
            string.IsNullOrWhiteSpace(
                settings.AdminUserName) ||
            string.IsNullOrWhiteSpace(
                settings.AdminEmail) ||
            string.IsNullOrWhiteSpace(
                settings.AdminPassword))
        {
            throw new InvalidOperationException(
                "IdentitySeed admin settings " +
                "are not configured.");
        }


        // =====================================================
        // Find Existing Admin
        // =====================================================

        var admin =
            await userManager.FindByNameAsync(
                settings.AdminUserName);


        admin ??=
            await userManager.FindByEmailAsync(
                settings.AdminEmail);


        // =====================================================
        // Create Admin
        // =====================================================

        if (admin is null)
        {
            admin =
                new AppUser
                {
                    UserName =
                        settings.AdminUserName,

                    Email =
                        settings.AdminEmail,

                    EmailConfirmed =
                        true,

                    ShortName =
                        "Administrator",

                    IsDisabled =
                        false,

                    ChangedHisOwnPassword =
                        false,

                    ChangedHisOwnNickName =
                        false
                };


            var createResult =
                await userManager.CreateAsync(
                    admin,
                    settings.AdminPassword);


            if (!createResult.Succeeded)
            {
                var errors =
                    string.Join(
                        " | ",
                        createResult.Errors
                            .Select(
                                x =>
                                    x.Description));


                throw new InvalidOperationException(
                    "Unable to create initial " +
                    $"admin account. {errors}");
            }
        }


        // =====================================================
        // Ensure ADMIN Role
        // =====================================================

        var isAdmin =
            await userManager.IsInRoleAsync(
                admin,
                AppRoles.Admin);


        if (!isAdmin)
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    admin,
                    AppRoles.Admin);


            if (!roleResult.Succeeded)
            {
                var errors =
                    string.Join(
                        " | ",
                        roleResult.Errors
                            .Select(
                                x =>
                                    x.Description));


                throw new InvalidOperationException(
                    "Unable to assign ADMIN role. " +
                    errors);
            }
        }
    }
}