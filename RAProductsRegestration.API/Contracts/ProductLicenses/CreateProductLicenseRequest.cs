using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.API.Contracts.ProductLicenses
{
    public sealed record CreateProductLicenseRequest(
        Guid ProductId,
        Guid LicenseCategoryId,
        DateOnly IssueDate,
        DateOnly ValidityDate,
        Guid MarketingTypeId,
        string RegistrationNumberYear);
    public sealed record UpdateLicenseCategoryRequest(
    Guid LicenseCategoryId);
    public sealed record UpdateMarketingTypeRequest(
    Guid MarketingTypeId);
    public sealed record UpdateRegistrationNumberRequest(
    string RegistrationNumberYear);
    public sealed record DatedAttachmentRequest(
    DateOnly Date,
    Guid AttachmentId,
    string FileName,
    string? Notes)
    {
        public DatedAttachmentInput ToApplication()
        {
            return new DatedAttachmentInput(
                Date,
                AttachmentId,
                FileName,
                Notes);
        }
    }
    public sealed record UpdateLicenseDatesRequest(
    DateOnly IssueDate,
    DateOnly ValidityDate);
}
