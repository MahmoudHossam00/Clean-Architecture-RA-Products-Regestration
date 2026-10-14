using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features
    .Features.ProductLicenses;
using RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.LicenseCategories;
using RAProductsRegestration.Domain.MarketingTypes;
using RAProductsRegestration.Domain.ProductLicenses;
using RAProductsRegestration.Domain.ProductLicenses.Requirements;
using LookupResponse = RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId.LookupResponse;

namespace RAProductsRegestration.Application.Features
    .ProductLicenses.Queries.GetByProductId;

internal static class ProductLicenseMapper
{
    public static ProductLicenseResponse Map(
        ProductLicense license,
        LicenseCategory licenseCategory,
        MarketingType marketingType,
        IReadOnlyCollection<Department> departments)
    {
        var departmentMap =
            departments.ToDictionary(
                x => x.Id);


        var requirements =
            license.Requirements
                .OrderBy(
                    x => x.Deadline)
                .Select(
                    requirement =>
                        MapRequirement(
                            requirement,
                            departmentMap))
                .ToArray();


        return new ProductLicenseResponse(
            license.Id,

            license.ProductId,

            new LookupResponse(
                licenseCategory.Id,
                licenseCategory.Name),

            license.IssueDate,

            license.ValidityDate,

            new LookupResponse(
                marketingType.Id,
                marketingType.Name),

            license.RegistrationNumberYear,

            MapDatedAttachment(
                license.LastApprovedLeaflet),

            MapDatedAttachment(
                license.LastApprovedLayout),

            requirements);
    }


    // =========================================================
    // Requirement
    // =========================================================

    private static LicenseRequirementResponse
        MapRequirement(
            LicenseRequirement requirement,
            IReadOnlyDictionary<
                Guid,
                Department> departments)
    {
        if (
            !departments.TryGetValue(
                requirement.ResponsibleDepartmentId,
                out var department)
        )
        {
            throw new InvalidOperationException(
                $"Department '{requirement.ResponsibleDepartmentId}' was not found.");
        }


        return new LicenseRequirementResponse(
            requirement.Id,

            requirement.Name,

            requirement.Deadline,

            new LookupResponse(
                department.Id,
                department.Name),

            MapAttachment(
                requirement.RequirementAttachment),

            MapDatedAttachment(
                requirement.Approval),

            requirement.Notes);
    }


    // =========================================================
    // Attachment
    // =========================================================

    private static AttachmentResponse?
        MapAttachment(
            AttachmentReference? attachment)
    {
        if (attachment is null)
        {
            return null;
        }


        return new AttachmentResponse(
            attachment.AttachmentId,
            attachment.FileName,
            attachment.Notes);
    }


    // =========================================================
    // Dated Attachment
    // =========================================================

    private static DatedAttachmentResponse?
        MapDatedAttachment(
            DatedAttachment? attachment)
    {
        if (attachment is null)
        {
            return null;
        }


        return new DatedAttachmentResponse(
            attachment.Date,

            new AttachmentResponse(
                attachment.Attachment
                    .AttachmentId,

                attachment.Attachment
                    .FileName,

                attachment.Attachment
                    .Notes));
    }
}