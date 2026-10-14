using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.AddApprovedPack
{
    public sealed record AddApprovedPackCommand(
    Guid ProductBasicDataId,
    string Description,
    DatedAttachmentInput Approval)
    : ICommand<Created>;
}
