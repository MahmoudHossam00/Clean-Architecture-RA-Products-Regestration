using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Security
{
    public static class AppRoles
    {
        public const string Admin =
            "ADMIN";

        public const string RA =
            "RA";

        public const string Viewer =
            "VIEWER";


        public static readonly string[] All =
        [
            Admin,
        RA,
        Viewer
        ];
    }
}
