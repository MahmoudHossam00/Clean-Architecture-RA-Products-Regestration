using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.AddApprovedManfacturer
{
    public sealed record AddApprovedManufacturerCommand(
    Guid ProductBasicDataId,
    string ManufacturerName,
    DatedAttachmentInput Approval)
    : ICommand<Created>;
}
