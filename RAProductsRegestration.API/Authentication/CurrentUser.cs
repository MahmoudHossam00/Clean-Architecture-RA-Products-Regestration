using System.Security.Claims;
using RAProductsRegestration.Application.Common.Interfaces.Authentication;

namespace RAProductsRegestration.API.Authentication;

internal sealed class CurrentUser(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUser
{
    private ClaimsPrincipal? User =>
        httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated == true;

    public Guid? UserId
    {
        get
        {
            var value =
                User?.FindFirstValue(
                    ClaimTypes.NameIdentifier)
                ?? User?.FindFirstValue("sub")
                ?? User?.FindFirstValue("userId");

            return Guid.TryParse(
                value,
                out var userId)
                ? userId
                : null;
        }
    }

    public string? UserName =>
        User?.FindFirstValue(ClaimTypes.Name)
        ?? User?.FindFirstValue("name")
        ?? User?.Identity?.Name;

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email)
        ?? User?.FindFirstValue("email");

    public IReadOnlyCollection<string> Roles =>
        User?
            .Claims
            .Where(x =>
                x.Type == ClaimTypes.Role ||
                x.Type == "role" ||
                x.Type == "roles")
            .Select(x => x.Value)
            .Where(x =>
                !string.IsNullOrWhiteSpace(x))
            .Distinct(
                StringComparer.OrdinalIgnoreCase)
            .ToArray()
        ?? [];

    public bool IsInRole(
        string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return false;
        }

        return Roles.Contains(
            role,
            StringComparer.OrdinalIgnoreCase);
    }
}