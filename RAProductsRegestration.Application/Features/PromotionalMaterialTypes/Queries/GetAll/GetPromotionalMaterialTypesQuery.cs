using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterialTypes.Queries.GetAll
{
    public sealed record GetPromotionalMaterialTypesQuery(
        string? Search)
        : IQuery<LookupResponse[]>;
}
