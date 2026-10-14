using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.ActivateApprovedManufacturer
{
    public sealed record ActivateApprovedManufacturerCommand(
    Guid ProductBasicDataId,
    Guid ApprovedManufacturerId)
    : ICommand<Updated>;
}
