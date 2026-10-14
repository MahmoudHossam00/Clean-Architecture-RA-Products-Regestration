using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Queries.GetByProductId
{
    public sealed record GetAppealsByProductIdQuery(
        Guid ProductId)
        : IQuery<AppealResponse[]>;
}
