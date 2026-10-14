using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.UpdateApprovedPack
{
    public sealed record UpdateApprovedPackCommand(
    Guid ProductBasicDataId,
    Guid ApprovedPackId,
    string Description,
    DatedAttachmentInput Approval)
    : ICommand<Updated>;
}
