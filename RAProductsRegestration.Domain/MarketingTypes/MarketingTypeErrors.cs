using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.MarketingTypes
{
    public static class MarketingTypeErrors
    {
        public static readonly Error NameRequired =
            Error.Validation(
                "MarketingType.NameRequired",
                "Marketing type name is required.");

        public static Error NotFound(
            Guid id) =>
            Error.NotFound(
                "MarketingType.NotFound",
                $"Marketing type '{id}' was not found.");

        public static Error AlreadyExists(
            string name) =>
            Error.Conflict(
                "MarketingType.AlreadyExists",
                $"Marketing type '{name}' already exists.");
    }
}
