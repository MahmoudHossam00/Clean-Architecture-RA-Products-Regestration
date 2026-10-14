namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record AuthenticationTokens(
        string AccessToken,
        string RefreshToken,
        DateTimeOffset AccessTokenExpiresAt);
}
