using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.AppealTypes.Commands.Queries.GetAll
{

    public sealed record GetAppealTypesQuery(
        string? Search)
        : IQuery<LookupResponse[]>;
}
