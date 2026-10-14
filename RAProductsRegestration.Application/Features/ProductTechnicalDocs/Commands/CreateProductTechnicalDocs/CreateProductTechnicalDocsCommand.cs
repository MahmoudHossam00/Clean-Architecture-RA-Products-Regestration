using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.CreateProductTechnicalDocs
{
    public sealed record CreateProductTechnicalDocsCommand(
        Guid ProductId)
        : ICommand<Created>;
}
