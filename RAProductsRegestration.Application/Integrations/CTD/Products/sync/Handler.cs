using RAProductsRegestration.Application.Common.Interfaces.Integrations.CTD;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Integrations.CTD;
using RAProductsRegestration.Application.Integrations.CTD.Products.sync;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Companies;
using RAProductsRegestration.Domain.Products;

internal sealed class Handler(
    ICTDDataSource ctdDataSource,
    IRepository<Product> productRepository,
    IRepository<Company> companyRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<SyncProductsCommand, Success>
{
    public async Task<Result<Success>> Handle(
        SyncProductsCommand request,
        CancellationToken cancellationToken)
    {
        var ctdProducts =
            await ctdDataSource.GetProductsAsync(
                cancellationToken);

        if (ctdProducts.Count == 0)
        {
            return Result.Success;
        }

        // Get distinct CTD product IDs
        var ctdProductIds = ctdProducts
            .Select(x => x.CTDId)
            .Distinct()
            .ToArray();

        // Get distinct company codes
        var companyCodes = ctdProducts
            .Select(x => x.CompanyCode)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        // One query for existing products
        var existingProducts =
            await productRepository.WhereAsync(
                x => ctdProductIds.Contains(x.CTDId),
                cancellationToken);

        // One query for companies
        var existingCompanies =
            await companyRepository.WhereAsync(
                x => companyCodes.Contains(x.Code),
                cancellationToken);

        // Fast in-memory lookup
        var productsByCTDId =
            existingProducts.ToDictionary(
                x => x.CTDId);

        var companiesByCode =
            existingCompanies.ToDictionary(
                x => x.Code,
                StringComparer.OrdinalIgnoreCase);

        foreach (var ctdProduct in ctdProducts)
        {
            if (!companiesByCode.TryGetValue(
                    ctdProduct.CompanyCode,
                    out var company))
            {
                return CTDIntegrationErrors
                    .CompanyNotFound(
                        ctdProduct.CompanyCode);
            }

            if (!productsByCTDId.TryGetValue(
                    ctdProduct.CTDId,
                    out var product))
            {
                product = new Product(
                    ctdProduct.CTDId,
                    ctdProduct.ProductCode,
                    ctdProduct.ProductName,
                    company.Id);

                 productRepository.Add(
                    product
                    );

                // Important if duplicated CTD IDs somehow
                // appear during the same synchronization.
                productsByCTDId.Add(
                    product.CTDId,
                    product);

                continue;
            }

            product.UpdateFromCTD(
                ctdProduct.ProductName,
                company.Id);
        }

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success;
    }
}