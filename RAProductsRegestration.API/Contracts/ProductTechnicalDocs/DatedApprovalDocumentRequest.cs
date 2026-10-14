using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.ProductTechnicalDocs
{
    public sealed record DatedApprovalDocumentRequest(
        DateOnly ApprovalDate,
        AttachmentReferenceInput Attachment);
}
