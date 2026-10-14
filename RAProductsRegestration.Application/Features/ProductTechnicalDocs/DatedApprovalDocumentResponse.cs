using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs
{

    public sealed record DatedApprovalDocumentResponse(
        DateOnly ApprovalDate,
        AttachmentResponse Attachment);
    public sealed record ProductTechnicalDocsResponse(
    Guid Id,
    Guid ProductId,

    DatedApprovalDocumentResponse?
        CurrentApprovedSpecifications,

    IReadOnlyCollection<DatedApprovalDocumentResponse>
        ApprovedSpecificationsHistory,

    DatedApprovalDocumentResponse?
        CurrentApprovedComposition,

    IReadOnlyCollection<DatedApprovalDocumentResponse>
        ApprovedCompositionHistory,

    DatedApprovalDocumentResponse?
        CurrentApprovedMoaValidation,

    IReadOnlyCollection<DatedApprovalDocumentResponse>
        ApprovedMoaValidationHistory);
}
