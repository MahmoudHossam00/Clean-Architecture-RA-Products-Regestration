using RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;

namespace RAProductsRegestration.Application.Features
    .Features.ProductLicenses;

public sealed record ProductLicenseResponse(
    Guid Id,
    Guid ProductId,

    LookupResponse LicenseCategory,

    DateOnly IssueDate,
    DateOnly ValidityDate,

    LookupResponse MarketingType,

    string RegistrationNumberYear,

    DatedAttachmentResponse? LastApprovedLeaflet,

    DatedAttachmentResponse? LastApprovedLayout,

    IReadOnlyCollection<LicenseRequirementResponse>
        Requirements);
