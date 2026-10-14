using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetShelfLifeHistory
{
    public sealed record GetShelfLifeHistoryQuery(
        Guid ProductBasicDataId)
        : IQuery<ShelfLifeHistoryResponse[]>;

    public sealed record ShelfLifeHistoryResponse(
    Guid Id,
    int VersionNumber,
    bool IsCurrent,
    int NumberOfMonths,
    DateOnly ApprovalDate,
    ShelfLifeHistoryAttachmentResponse[] Attachments);


    public sealed record ShelfLifeHistoryAttachmentResponse(
        Guid AttachmentId,
        string FileName,
        string? Notes);
}
