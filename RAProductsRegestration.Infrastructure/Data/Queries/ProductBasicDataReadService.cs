using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Queries;
using RAProductsRegestration.Application.Features
    .ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Domain.ActiveIngredients;
using RAProductsRegestration.Domain.DosageForms;
using RAProductsRegestration.Domain.StorageConditions;
using RAProductsRegestration.Domain.Suppliers;

namespace RAProductsRegestration.Infrastructure.Data.Queries;

internal sealed class ProductBasicDataReadService(
    ApplicationDbContext context)
    : IProductBasicDataReadService
{
    public async Task<ProductBasicDataResponse?>
        GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
    {
        var basicData =
            await context.ProductBasicData
                .AsNoTracking()
                .AsSplitQuery()

                // =================================================
                // Active Ingredients
                // =================================================

                .Include(x =>
                    x.ProductActiveIngredients)
                    .ThenInclude(x =>
                        x.ApprovedSuppliers)

                .Include(x =>
                    x.ProductActiveIngredients)
                    .ThenInclude(x =>
                        x.ApprovedPacks)

                // =================================================
                // Product Level Data
                // =================================================

                .Include(x =>
                    x.ApprovedPacks)

                .Include(x =>
                    x.ApprovedPrices)

                .Include(x =>
                    x.ApprovedManufacturers)
                // =================================================
                // Shelf Life History
                // =================================================
                
                .Include(x =>
                    x.ShelfLifeHistory)

                // =================================================
                // PV History
                // =================================================

                .Include(x =>
                    x.PVHistory)

                // =================================================
                // Decree 600 History
                // =================================================

                .Include(x =>
                    x.Decree600History)

                .FirstOrDefaultAsync(
                    x =>
                        x.ProductId ==
                        productId,
                    cancellationToken);


        if (basicData is null)
        {
            return null;
        }


        // =====================================================
        // Product
        // =====================================================

        var product =
            await context.Products
                .AsNoTracking()
                .Where(x =>
                    x.Id == productId)
                .Select(x => new
                {
                    x.ProductCode,
                    x.ProductName
                })
                .FirstAsync(
                    cancellationToken);


        // =====================================================
        // Dosage Form
        // =====================================================

        LookupResponse? dosageForm =
            null;


        if (basicData.DosageFormId.HasValue)
        {
            dosageForm =
                await context
                    .Set<DosageForm>()
                    .AsNoTracking()
                    .Where(x =>
                        x.Id ==
                        basicData.DosageFormId.Value)
                    .Select(x =>
                        new LookupResponse(
                            x.Id,
                            x.Name))
                    .FirstOrDefaultAsync(
                        cancellationToken);
        }


        // =====================================================
        // Storage Conditions
        // =====================================================

        var storageConditionIds =
            basicData.ProductActiveIngredients
                .Where(x =>
                    x.StorageConditionId.HasValue)
                .Select(x =>
                    x.StorageConditionId!.Value)
                .ToHashSet();


        if (basicData.StorageConditionId.HasValue)
        {
            storageConditionIds.Add(
                basicData.StorageConditionId.Value);
        }


        var storageConditions =
            await context
                .Set<StorageCondition>()
                .AsNoTracking()
                .Where(x =>
                    storageConditionIds.Contains(
                        x.Id))
                .Select(x =>
                    new LookupResponse(
                        x.Id,
                        x.Name))
                .ToDictionaryAsync(
                    x => x.Id,
                    cancellationToken);


        // =====================================================
        // Active Ingredient Master Data
        // =====================================================

        var activeIngredientIds =
            basicData.ProductActiveIngredients
                .Select(x =>
                    x.ActiveIngredientId)
                .ToHashSet();


        var activeIngredients =
            await context
                .Set<ActiveIngredient>()
                .AsNoTracking()
                .Where(x =>
                    activeIngredientIds.Contains(
                        x.Id))
                .Select(x =>
                    new LookupResponse(
                        x.Id,
                        x.Name))
                .ToDictionaryAsync(
                    x => x.Id,
                    cancellationToken);


        // =====================================================
        // Suppliers
        // =====================================================

        var supplierIds =
            basicData.ProductActiveIngredients
                .SelectMany(x =>
                    x.ApprovedSuppliers)
                .Select(x =>
                    x.SupplierId)
                .ToHashSet();


        var suppliers =
            await context
                .Set<Supplier>()
                .AsNoTracking()
                .Where(x =>
                    supplierIds.Contains(
                        x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.JdeCode,
                    x.Name
                })
                .ToDictionaryAsync(
                    x => x.Id,
                    cancellationToken);


        // =====================================================
        // Build Active Ingredients
        // =====================================================

        var activeIngredientResponses =
            basicData.ProductActiveIngredients
                .Select(ingredient =>
                {
                    var ingredientMaster =
                        activeIngredients[
                            ingredient.ActiveIngredientId];


                    LookupResponse?
                        ingredientStorageCondition =
                            null;


                    if (
                        ingredient
                            .StorageConditionId
                            .HasValue
                    )
                    {
                        storageConditions
                            .TryGetValue(
                                ingredient
                                    .StorageConditionId
                                    .Value,
                                out
                                    ingredientStorageCondition);
                    }


                    var approvedSuppliers =
                        ingredient.ApprovedSuppliers
                            .Where(x =>
                                suppliers.ContainsKey(
                                    x.SupplierId))
                            .Select(x =>
                            {
                                var supplier =
                                    suppliers[
                                        x.SupplierId];


                                return new ApprovedSupplierResponse(
                                    x.Id,
                                    x.SupplierId,
                                    supplier.JdeCode,
                                    supplier.Name,
                                    x.IsActive);
                            })
                            .ToList();


                    var approvedPacks =
                        ingredient.ApprovedPacks
                            .Select(x =>
                                MapPack(x))
                            .ToList();


                    return new ProductActiveIngredientResponse(
                        ingredient.Id,
                        ingredientMaster,
                        ingredient.Strength,
                        ingredientStorageCondition,
                        ingredient.PhysicalDescription,
                        MapShelfLife(
                            ingredient.ShelfLife),
                        approvedSuppliers,
                        approvedPacks);
                })
                .ToList();


        // =====================================================
        // Product Storage Condition
        // =====================================================

        LookupResponse?
            productStorageCondition =
                null;


        if (basicData.StorageConditionId.HasValue)
        {
            storageConditions.TryGetValue(
                basicData.StorageConditionId.Value,
                out productStorageCondition);
        }


        // =====================================================
        // PV Current + History
        // =====================================================

        var currentPV =
            basicData.CurrentPV is null
                ? null
                : MapDeadlineHistory(
                    basicData.CurrentPV.Id,
                    basicData.CurrentPV.VersionNumber,
                    basicData.CurrentPV.Deadline);


        var pvHistory =
            basicData.PVHistory
                .OrderByDescending(x =>
                    x.VersionNumber)
                .Select(x =>
                    MapDeadlineHistory(
                        x.Id,
                        x.VersionNumber,
                        x.Deadline))
                .ToArray();


        // =====================================================
        // Decree 600 Current + History
        // =====================================================

        var currentDecree600 =
            basicData.CurrentDecree600 is null
                ? null
                : MapDeadlineHistory(
                    basicData.CurrentDecree600.Id,
                    basicData.CurrentDecree600.VersionNumber,
                    basicData.CurrentDecree600.Deadline);


        var decree600History =
            basicData.Decree600History
                .OrderByDescending(x =>
                    x.VersionNumber)
                .Select(x =>
                    MapDeadlineHistory(
                        x.Id,
                        x.VersionNumber,
                        x.Deadline))
                .ToArray();


        // =====================================================
        // Final Response
        // =====================================================

        return new ProductBasicDataResponse(
            basicData.Id,

            basicData.ProductId,

            product.ProductCode,

            product.ProductName,

            basicData.TradeName,

            dosageForm,

            productStorageCondition,

            basicData.PhysicalDescription,

            MapShelfLife(
                basicData.CurrentShelfLife),

            // =================================================
            // PV
            // =================================================

            currentPV,

            pvHistory,

            // =================================================
            // Decree 600
            // =================================================

            currentDecree600,

            decree600History,

            // =================================================
            // Active Ingredients
            // =================================================

            activeIngredientResponses,

            // =================================================
            // Approved Packs
            // =================================================

            basicData.ApprovedPacks
                .Select(
                    MapPack)
                .ToList(),

            // =================================================
            // Approved Prices
            // =================================================

            basicData.ApprovedPrices
                .OrderByDescending(x =>
                    x.IssueDate)
                .Select(x =>
                    new ApprovedPriceResponse(
                        x.Id,
                        x.Price,
                        x.PricePerDescription,
                        x.IssueDate,
                        x.ExpiryDate))
                .ToList(),

            // =================================================
            // Approved Manufacturers
            // =================================================

            basicData.ApprovedManufacturers
                .Select(x =>
                    new ApprovedManufacturerResponse(
                        x.Id,
                        x.ManufacturerName,
                        x.IsActive,
                        MapDatedAttachment(
                            x.DatedAttachment)!))
                .ToList());
    }


    // =========================================================
    // Approved Pack
    // =========================================================

    private static ApprovedPackResponse MapPack(
        Domain.ProductBasicDatas
            .ApprovedPacks.ApprovedPack pack)
    {
        return new ApprovedPackResponse(
            pack.Id,
            pack.Description,
            MapDatedAttachment(
                pack.Approval)!);
    }


    // =========================================================
    // Shelf Life
    // =========================================================

    private static ShelfLifeResponse?
        MapShelfLife(
            Domain.Common.ValueObjects
                .ShelfLife? value)
    {
        if (value is null)
        {
            return null;
        }


        return new ShelfLifeResponse(
            value.NumberOfMonths,
            value.ApprovalDate,

            value.Attachments
                .Select(attachment =>
                    new AttachmentResponse(
                        attachment.AttachmentId,
                        attachment.FileName,
                        attachment.Notes))
                .ToList());
    }


    // =========================================================
    // Dated Attachment
    // =========================================================

    private static DatedAttachmentResponse?
        MapDatedAttachment(
            Domain.Common.ValueObjects
                .DatedAttachment? value)
    {
        if (value is null)
        {
            return null;
        }


        return new DatedAttachmentResponse(
            value.Date,

            new AttachmentResponse(
                value.Attachment.AttachmentId,
                value.Attachment.FileName,
                value.Attachment.Notes));
    }


    // =========================================================
    // PV / Decree 600 History
    // =========================================================

    private static FulfillmentDeadlineHistoryResponse
        MapDeadlineHistory(
            Guid id,
            int versionNumber,
            Domain.Common.ValueObjects
                .FulfillmentDeadline value)
    {
        return new FulfillmentDeadlineHistoryResponse(
            id,
            versionNumber,
            value.Description,
            value.Deadline,
            value.Notes,
            value.FulfillmentDate);
    }
}