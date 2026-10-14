using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.LicenseCategories.Queries.GetAll
{
    public sealed record GetLicenseCategoriesQuery(
        string? Search)
        : IQuery<IReadOnlyCollection<LookupResponse>>;
}
