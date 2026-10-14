using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateShelfLife
{

    public sealed record UpdateShelfLifeCommand(
        Guid ProductBasicDataId,
        int NumberOfMonths,
        DateOnly ApprovalDate,
        IReadOnlyCollection<AttachmentReferenceInput> Attachments)
        : ICommand<Created>;
}
