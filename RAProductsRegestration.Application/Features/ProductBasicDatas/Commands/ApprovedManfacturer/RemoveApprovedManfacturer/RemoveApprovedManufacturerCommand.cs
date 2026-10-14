using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.RemoveApprovedManfacturer
{
    public sealed record RemoveApprovedManufacturerCommand(
    Guid ProductBasicDataId,
    Guid ApprovedManufacturerId)
    : ICommand<Deleted>;
}
