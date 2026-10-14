using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.ProductsTechnicalDocs
{
    public static class ProductTechnicalDocsErrors
    {
        public static readonly Error ProductIdRequired =
            Error.Validation(
                "ProductTechnicalDocs.ProductIdRequired",
                "Product id is required.");
        public static Error AlreadyExistsForProduct(
    Guid productId) =>
    Error.Conflict(
        "ProductTechnicalDocs.AlreadyExistsForProduct",
        $"Product technical documents already exist for product '{productId}'.");
        public static Error NotFound(
    Guid id) =>
    Error.NotFound(
        "ProductTechnicalDocs.NotFound",
        $"Product technical documents '{id}' were not found.");

        public static Error NotFoundForProduct(
    Guid productId) =>
    Error.NotFound(
        "ProductTechnicalDocs.NotFoundForProduct",
        $"Product technical documents were not found for product '{productId}'.");
    }
}
