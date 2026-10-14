using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.ProductLicenses.Requirements;
using System.Text.RegularExpressions;

namespace RAProductsRegestration.Domain.ProductLicenses;

public sealed class ProductLicense
    : AuditableEntity, IAggregateRoot
{
    private ProductLicense()
    {
    }


    private ProductLicense(
        Guid productId,
        Guid licenseCategoryId,
        DateOnly issueDate,
        DateOnly validityDate,
        Guid marketingTypeId,
        string registrationNumberYear)
        : base(Guid.NewGuid())
    {
        ProductId =
            productId;

        LicenseCategoryId =
            licenseCategoryId;

        IssueDate =
            issueDate;

        ValidityDate =
            validityDate;

        MarketingTypeId =
            marketingTypeId;

        RegistrationNumberYear =
            registrationNumberYear;
    }


    // =========================================================
    // Basic Information
    // =========================================================

    public Guid ProductId
    {
        get;
        private set;
    }
    private readonly List<LicenseRequirement>
    _requirements = [];


    public IReadOnlyCollection<LicenseRequirement>
        Requirements =>
            _requirements.AsReadOnly();
    public Result<Created> AddRequirement(
    string name,
    DateOnly deadline,
    Guid responsibleDepartmentId,
    AttachmentReference? requirementAttachment,
    string? notes)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return LicenseRequirementErrors
                .NameRequired;
        }


        if (responsibleDepartmentId == Guid.Empty)
        {
            return LicenseRequirementErrors
                .DepartmentRequired;
        }


        var requirement =
            new LicenseRequirement(
                name.Trim(),
                deadline,
                responsibleDepartmentId,
                requirementAttachment,
                notes);


        _requirements.Add(
            requirement);


        return Result.Created;
    }
    public Result<Updated> UpdateRequirement(
    Guid requirementId,
    string name,
    DateOnly deadline,
    Guid responsibleDepartmentId,
    string? notes)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return LicenseRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .UpdateDetails(
                name,
                deadline,
                responsibleDepartmentId,
                notes);
    }
    public Result<Updated>
    SetRequirementAttachment(
        Guid requirementId,
        AttachmentReference attachment)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return LicenseRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .SetRequirementAttachment(
                attachment);
    }
    public Result<Updated>
    RemoveRequirementAttachment(
        Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return LicenseRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .RemoveRequirementAttachment();
    }
    public Result<Updated>
    SetRequirementApproval(
        Guid requirementId,
        DateOnly approvalDate,
        AttachmentReference attachment)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return LicenseRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .SetApproval(
                approvalDate,
                attachment);
    }
    public Result<Updated>
    RemoveRequirementApproval(
        Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return LicenseRequirementErrors
                .NotFound(
                    requirementId);
        }


        return requirement
            .RemoveApproval();
    }
    public Result<Deleted>
    RemoveRequirement(
        Guid requirementId)
    {
        var requirement =
            FindRequirement(
                requirementId);


        if (requirement is null)
        {
            return LicenseRequirementErrors
                .NotFound(
                    requirementId);
        }


        _requirements.Remove(
            requirement);


        return Result.Deleted;
    }
    private LicenseRequirement?
    FindRequirement(
        Guid requirementId)
    {
        return _requirements
            .FirstOrDefault(
                x =>
                    x.Id ==
                    requirementId);
    }

    public Guid LicenseCategoryId
    {
        get;
        private set;
    }


    public DateOnly IssueDate
    {
        get;
        private set;
    }


    public DateOnly ValidityDate
    {
        get;
        private set;
    }


    public Guid MarketingTypeId
    {
        get;
        private set;
    }


    public string RegistrationNumberYear
    {
        get;
        private set;
    } = null!;


    // =========================================================
    // Last Approved Documents
    // =========================================================

    public DatedAttachment?
        LastApprovedLeaflet
    {
        get;
        private set;
    }


    public DatedAttachment?
        LastApprovedLayout
    {
        get;
        private set;
    }


    // =========================================================
    // Create
    // =========================================================

    public static Result<ProductLicense> Create(
        Guid productId,
        Guid licenseCategoryId,
        DateOnly issueDate,
        DateOnly validityDate,
        Guid marketingTypeId,
        string registrationNumberYear)
    {
        if (productId == Guid.Empty)
        {
            return ProductLicenseErrors
                .ProductIdRequired;
        }


        if (licenseCategoryId == Guid.Empty)
        {
            return ProductLicenseErrors
                .LicenseCategoryRequired;
        }


        if (marketingTypeId == Guid.Empty)
        {
            return ProductLicenseErrors
                .MarketingTypeRequired;
        }


        if (validityDate < issueDate)
        {
            return ProductLicenseErrors
                .ValidityDateBeforeIssueDate;
        }


        var registrationResult =
            ValidateRegistrationNumberYear(
                registrationNumberYear);

        if (registrationResult.IsError)
        {
            return registrationResult.TopError!;
        }


        return new ProductLicense(
            productId,
            licenseCategoryId,
            issueDate,
            validityDate,
            marketingTypeId,
            registrationNumberYear.Trim());
    }


    // =========================================================
    // License Category
    // =========================================================

    public Result<Updated>
        UpdateLicenseCategory(
            Guid licenseCategoryId)
    {
        if (licenseCategoryId == Guid.Empty)
        {
            return ProductLicenseErrors
                .LicenseCategoryRequired;
        }


        LicenseCategoryId =
            licenseCategoryId;


        return Result.Updated;
    }


    // =========================================================
    // License Dates
    // =========================================================

    public Result<Updated>
        UpdateLicenseDates(
            DateOnly issueDate,
            DateOnly validityDate)
    {
        if (validityDate < issueDate)
        {
            return ProductLicenseErrors
                .ValidityDateBeforeIssueDate;
        }


        IssueDate =
            issueDate;


        ValidityDate =
            validityDate;


        return Result.Updated;
    }


    // =========================================================
    // Marketing Type
    // =========================================================

    public Result<Updated>
        UpdateMarketingType(
            Guid marketingTypeId)
    {
        if (marketingTypeId == Guid.Empty)
        {
            return ProductLicenseErrors
                .MarketingTypeRequired;
        }


        MarketingTypeId =
            marketingTypeId;


        return Result.Updated;
    }


    // =========================================================
    // Registration Number / Year
    // =========================================================

    public Result<Updated>
        UpdateRegistrationNumberYear(
            string registrationNumberYear)
    {
        var result =
            ValidateRegistrationNumberYear(
                registrationNumberYear);


        if (result.IsError)
        {
            return result.TopError!;
        }


        RegistrationNumberYear =
            registrationNumberYear.Trim();


        return Result.Updated;
    }


    // =========================================================
    // Last Approved Leaflet
    // =========================================================

    public Result<Updated>
        SetLastApprovedLeaflet(
            DateOnly approvalDate,
            AttachmentReference attachment)
    {
        ArgumentNullException.ThrowIfNull(
            attachment);


        LastApprovedLeaflet =
            new DatedAttachment(
                approvalDate,
                attachment);


        return Result.Updated;
    }


    // =========================================================
    // Last Approved Layout
    // =========================================================

    public Result<Updated>
        SetLastApprovedLayout(
            DateOnly approvalDate,
            AttachmentReference attachment)
    {
        ArgumentNullException.ThrowIfNull(
            attachment);


        LastApprovedLayout =
            new DatedAttachment(
                approvalDate,
                attachment);


        return Result.Updated;
    }


    // =========================================================
    // Registration Number Validation
    // =========================================================

    private static Result<Success>
        ValidateRegistrationNumberYear(
            string registrationNumberYear)
    {
        if (string.IsNullOrWhiteSpace(
            registrationNumberYear))
        {
            return ProductLicenseErrors
                .RegistrationNumberRequired;
        }


        var value =
            registrationNumberYear.Trim();


        /*
         * Valid:
         *
         * 123/2026
         * 12345/2026
         * 10/25
         *
         * Invalid:
         *
         * ABC/2026
         * 123-
         * /2026
         * 123/ABC
         */
        if (!Regex.IsMatch(
            value,
            @"^\d+/\d+$"))
        {
            return ProductLicenseErrors
                .InvalidRegistrationNumber;
        }


        return Result.Success;
    }
}