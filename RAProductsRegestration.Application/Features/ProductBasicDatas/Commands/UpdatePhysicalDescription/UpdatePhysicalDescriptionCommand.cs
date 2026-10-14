using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdatePhysicalDescription
{
    public sealed record UpdatePhysicalDescriptionCommand(
    Guid ProductBasicDataId,
    string? PhysicalDescription)
    : ICommand<Updated>;
}
