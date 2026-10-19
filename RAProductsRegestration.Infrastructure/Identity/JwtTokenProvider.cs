using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Identity
{
    internal sealed class JwtTokenProvider(
    IOptions<JwtSettings> options)
    {
        private readonly JwtSettings _settings =
            options.Value;


        public (
            string Token,
            DateTimeOffset ExpiresAtUtc)
            Generate(
                AppUser user,
                IEnumerable<string> roles)
        {
            var now =
                DateTimeOffset.UtcNow;

            var expiresAt =
                now.AddMinutes(
                    _settings.ExpirationMinutes);


            var claims =
                new List<Claim>
                {
                new(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new(
                    JwtRegisteredClaimNames.Sub,
                    user.Id),

                new(
                    ClaimTypes.Name,
                    user.UserName ?? string.Empty),

                new(
                    JwtRegisteredClaimNames.UniqueName,
                    user.UserName ?? string.Empty)
                };


            if (
                !string.IsNullOrWhiteSpace(
                    user.Email)
            )
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Email,
                        user.Email));
            }


            if (
                !string.IsNullOrWhiteSpace(
                    user.ShortName)
            )
            {
                claims.Add(
                    new Claim(
                        "short_name",
                        user.ShortName));
            }


            if (
                user.EmployeeId.HasValue
            )
            {
                claims.Add(
                    new Claim(
                        "employee_id",
                        user.EmployeeId.Value
                            .ToString()));
            }


            foreach (
                var role
                in roles
            )
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }


            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _settings.Key));


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);


            var token =
                new JwtSecurityToken(
                    issuer:
                        _settings.Issuer,

                    audience:
                        _settings.Audience,

                    claims:
                        claims,

                    notBefore:
                        now.UtcDateTime,

                    expires:
                        expiresAt.UtcDateTime,

                    signingCredentials:
                        credentials);


            return (
                new JwtSecurityTokenHandler()
                    .WriteToken(
                        token),

                expiresAt);
        }
    }
}
