using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductLicenses.Queries.GetByProductId;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.LicenseCategories;
using RAProductsRegestration.Domain.MarketingTypes;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Queries.GetProductLicenseByProductId
{
    internal sealed class Handler(
        IProductLicenseRepository licenseRepository,
        IRepository<LicenseCategory> categoryRepository,
        IRepository<MarketingType> marketingTypeRepository,
        IRepository<Department> departmentRepository)
        : IQueryHandler<
            GetProductLicenseByProductIdQuery,
            ProductLicenseResponse>
    {
        public async Task<Result<ProductLicenseResponse>> Handle(
            GetProductLicenseByProductIdQuery request,
            CancellationToken ct)
        {
            // =====================================================
            // License + Requirements
            // =====================================================

            var license =
                await licenseRepository
                    .GetByProductIdWithRequirementsAsync(
                        request.ProductId,
                        ct);


            if (license is null)
            {
                return ProductLicenseErrors
                    .NotFoundForProduct(
                        request.ProductId);
            }


            // =====================================================
            // License Category
            // =====================================================

            var category =
                await categoryRepository
                    .GetByIdAsync(
                        license.LicenseCategoryId,
                        ct);


            if (category is null)
            {
                return LicenseCategoryErrors
                    .NotFound(
                        license.LicenseCategoryId);
            }


            // =====================================================
            // Marketing Type
            // =====================================================

            var marketingType =
                await marketingTypeRepository
                    .GetByIdAsync(
                        license.MarketingTypeId,
                        ct);


            if (marketingType is null)
            {
                return MarketingTypeErrors
                    .NotFound(
                        license.MarketingTypeId);
            }


            // =====================================================
            // Departments used by Requirements
            // =====================================================

            var departmentIds =
                license.Requirements
                    .Select(
                        x =>
                            x.ResponsibleDepartmentId)
                    .Distinct()
                    .ToArray();


            var departments =
                departmentIds.Length == 0

                    ? []

                    : await departmentRepository
                        .WhereAsync(
                            x =>
                                departmentIds.Contains(
                                    x.Id),
                            ct);


            // =====================================================
            // Map
            // =====================================================

            return ProductLicenseMapper.Map(
                license,
                category,
                marketingType,
                departments);
        }
    }
}
