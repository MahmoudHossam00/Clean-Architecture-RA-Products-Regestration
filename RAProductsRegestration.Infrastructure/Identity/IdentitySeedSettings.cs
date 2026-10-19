using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Infrastructure.Identity
{
    public sealed class IdentitySeedSettings
    {
        public const string SectionName =
            "IdentitySeed";


        public string AdminUserName
        {
            get;
            init;
        } = null!;


        public string AdminEmail
        {
            get;
            init;
        } = null!;


        public string AdminPassword
        {
            get;
            init;
        } = null!;
    }
}
