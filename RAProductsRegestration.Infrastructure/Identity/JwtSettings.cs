using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Identity
{
    public sealed class JwtSettings
    {
        public const string SectionName =
            "Jwt";

        public string Issuer
        {
            get;
            init;
        } = null!;

        public string Audience
        {
            get;
            init;
        } = null!;

        public string Key
        {
            get;
            init;
        } = null!;

        public int ExpirationMinutes
        {
            get;
            init;
        } = 480;
    }
}
