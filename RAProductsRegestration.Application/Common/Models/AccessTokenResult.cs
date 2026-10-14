namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record AccessTokenResult(
    string AccessToken,
    DateTimeOffset ExpiresAt);
}
