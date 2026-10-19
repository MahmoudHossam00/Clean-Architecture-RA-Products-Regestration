using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAProductsRegestration.API.Contracts.ProductBasicDatas;
using RAProductsRegestration.API.Contracts.ProductBasicDatas;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.ActivateApprovedSupplier;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.r;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.ActivateApprovedSupplier;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredientApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddApprovedSupplier;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddApprovedSupplier;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddShelfLifeAttachment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.DeactivateApprovedSupplier;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.DeactivateApprovedSupplier;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.RemoveActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.RemoveActiveIngredient;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.RemoveActiveIngredientApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.RemoveActiveIngredientApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.RemoveShelfLifeAttachment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientPhysicalDescription;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientPhysicalDescription;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientShelfLife;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientShelfLife;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStorageCondition;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStorageCondition;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStrength;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStrength;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.AddShelfLifeAttachment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.ActivateApprovedManufacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.ActivateApprovedManufacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.AddApprovedManfacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.AddApprovedManfacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.DeactivateApprovedManufacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.DeactivateApprovedManufacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.RemoveApprovedManfacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedManfacturer.RemoveApprovedManfacturer;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.AddApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.AddApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.UpdateApprovedPack;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.remvoe;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.UpdateApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPacks.UpdateApprovedPack;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.AddApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.AddApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.RemoveApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.RemoveApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.UpdateApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.UpdateApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.AddDecree600;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.FulfillDecree600;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.FulfillDecree600;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.ResetDecree600Fulfillment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.ResetDecree600Fulfillment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.AddPV;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.FulfillPV;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.SetDecree600;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.SetDecree600;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.UpdateDecree600;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.UpdateDecree600;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.FulfillPV;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.FulfillPV;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.FulfillPV;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.ResetPVFulfillment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.ResetPVFulfillment;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.SetPV;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.SetPV;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.UpdatePV;
//using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.UpdatePV;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.RemoveShelfLifeAttachment;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateDosageForm;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdatePhysicalDescription;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdatePhysicalDescription;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateShelfLife;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateShelfLife;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateStorageCondition;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateTradeName;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateTradeName;
// Adjust these namespaces to match your exact folders
using RAProductsRegestration.Application.Features.ProductBasicDatas.Create;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetShelfLifeHistory;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/product-basic-data")]
public sealed class ProductBasicDataController(
    ISender sender)
    : ControllerBase
{
    // =========================================================
    // Create
    // =========================================================

    [HttpPost]
    public Task<Result<Created>> Create(
        CreateProductBasicDataRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new CreateProductBasicDataCommand(
                request.ProductId,
                request.TradeName),
            ct);
    }


    // =========================================================
    // General Basic Data
    // =========================================================

    [HttpPut("{id:guid}/trade-name")]
    public Task<Result<Updated>> UpdateTradeName(
        Guid id,
        UpdateTradeNameRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateTradeNameCommand(
                id,
                request.TradeName),
            ct);
    }


    [HttpPut("{id:guid}/dosage-form")]
    public Task<Result<Updated>> UpdateDosageForm(
        Guid id,
        UpdateDosageFormRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateDosageFormCommand(
                id,
                request.DosageFormId),
            ct);
    }


    [HttpPut("{id:guid}/storage-condition")]
    public Task<Result<Updated>> UpdateStorageCondition(
        Guid id,
        UpdateStorageConditionRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateStorageConditionCommand(
                id,
                request.StorageConditionId),
            ct);
    }


    [HttpPut("{id:guid}/physical-description")]
    public Task<Result<Updated>> UpdatePhysicalDescription(
        Guid id,
        UpdatePhysicalDescriptionRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdatePhysicalDescriptionCommand(
                id,
                request.PhysicalDescription),
            ct);
    }
    [HttpGet(
    "{productBasicDataId:guid}/shelf-life/history")]
    public async Task<IActionResult> GetShelfLifeHistory(
    Guid productBasicDataId,
    CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new GetShelfLifeHistoryQuery(
                    productBasicDataId),
                cancellationToken);

        return result.OnMatch<IActionResult>(
                       success => Ok(success),
                       errors => BadRequest(new
                       {
                           errors = errors.Select(error => new
                           {
                               error.Code,
                               error.Description
                           })
                       }));
    }

    [HttpPut("{id:guid}/shelf-life")]
    public Task<Result<Created>> UpdateShelfLife(
        Guid id,
        UpdateShelfLifeRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateShelfLifeCommand(
                id,
                request.ShelfLife.NumberOfMonths,
                request.ShelfLife.ApprovalDate,
                request.ShelfLife.Attachments),
            ct);
    }


    [HttpPost(
    "{id:guid}/shelf-life/attachments")]
    public Task<Result<Created>>
    AddShelfLifeAttachment(
        Guid id,
        AttachmentReferenceInput request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddShelfLifeAttachmentCommand(
                id,
                request),
            ct);
    }


    [HttpDelete(
        "{id:guid}/shelf-life/attachments/{attachmentId:guid}")]
    public Task<Result<Deleted>>
        RemoveShelfLifeAttachment(
            Guid id,
            Guid attachmentId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveShelfLifeAttachmentCommand(
                id,
                attachmentId),
            ct);
    }

    // =========================================================
    // Active Ingredients
    // =========================================================


    [HttpPost(
    "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/shelf-life/attachments")]
    public Task<Result<Created>>
    AddActiveIngredientShelfLifeAttachment(
        Guid id,
        Guid productActiveIngredientId,
        AttachmentReferenceInput request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddActiveIngredientShelfLifeAttachmentCommand(
                id,
                productActiveIngredientId,
                request),
            ct);
    }


    [HttpDelete(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/shelf-life/attachments/{attachmentId:guid}")]
    public Task<Result<Deleted>>
        RemoveActiveIngredientShelfLifeAttachment(
            Guid id,
            Guid productActiveIngredientId,
            Guid attachmentId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveActiveIngredientShelfLifeAttachmentCommand(
                id,
                productActiveIngredientId,
                attachmentId),
            ct);
    }


    [HttpPost("{id:guid}/active-ingredients")]
    public Task<Result<Created>> AddActiveIngredient(
        Guid id,
        AddActiveIngredientRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddActiveIngredientCommand(
                id,
                request.ActiveIngredientId,
                request.Strength),
            ct);
    }


    [HttpPut(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/strength")]
    public Task<Result<Updated>> UpdateActiveIngredientStrength(
        Guid id,
        Guid productActiveIngredientId,
        UpdateStrengthRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateActiveIngredientStrengthCommand(
                id,
                productActiveIngredientId,
                request.Strength),
            ct);
    }


    [HttpPut(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/storage-condition")]
    public Task<Result<Updated>>
        UpdateActiveIngredientStorageCondition(
            Guid id,
            Guid productActiveIngredientId,
            UpdateStorageConditionRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateActiveIngredientStorageConditionCommand(
                id,
                productActiveIngredientId,
                request.StorageConditionId),
            ct);
    }


    [HttpPut(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/physical-description")]
    public Task<Result<Updated>>
        UpdateActiveIngredientPhysicalDescription(
            Guid id,
            Guid productActiveIngredientId,
            UpdatePhysicalDescriptionRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateActiveIngredientPhysicalDescriptionCommand(
                id,
                productActiveIngredientId,
                request.PhysicalDescription),
            ct);
    }


    [HttpPut(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/shelf-life")]
    public Task<Result<Updated>>
        UpdateActiveIngredientShelfLife(
            Guid id,
            Guid productActiveIngredientId,
            UpdateShelfLifeRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateActiveIngredientShelfLifeCommand(
                id,
                productActiveIngredientId,
                request.ShelfLife.NumberOfMonths,
                request.ShelfLife.ApprovalDate,
                request.ShelfLife.Attachments
            ),
            ct
        );
    }


    [HttpDelete(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}")]
    public Task<Result<Deleted>> RemoveActiveIngredient(
        Guid id,
        Guid productActiveIngredientId,
        CancellationToken ct)
    {
        return sender.Send(
            new RemoveActiveIngredientCommand(
                id,
                productActiveIngredientId),
            ct);
    }


    // =========================================================
    // Approved Suppliers
    // =========================================================

    [HttpPost(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/suppliers")]
    public Task<Result<Created>> AddApprovedSupplier(
        Guid id,
        Guid productActiveIngredientId,
        AddApprovedSupplierRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddApprovedSupplierCommand(
                id,
                productActiveIngredientId,
                request.SupplierId),
            ct);
    }


    [HttpPatch(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/suppliers/{approvedSupplierId:guid}/activate")]
    public Task<Result<Updated>> ActivateApprovedSupplier(
        Guid id,
        Guid productActiveIngredientId,
        Guid approvedSupplierId,
        CancellationToken ct)
    {
        return sender.Send(
            new ActivateApprovedSupplierCommand(
                id,
                productActiveIngredientId,
                approvedSupplierId),
            ct);
    }


    [HttpPatch(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/suppliers/{approvedSupplierId:guid}/deactivate")]
    public Task<Result<Updated>> DeactivateApprovedSupplier(
        Guid id,
        Guid productActiveIngredientId,
        Guid approvedSupplierId,
        CancellationToken ct)
    {
        return sender.Send(
            new DeactivateApprovedSupplierCommand(
                id,
                productActiveIngredientId,
                approvedSupplierId),
            ct);
    }


    //[HttpDelete(
    //    "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/suppliers/{approvedSupplierId:guid}")]
    //public Task<Result<Deleted>> RemoveApprovedSupplier(
    //    Guid id,
    //    Guid productActiveIngredientId,
    //    Guid approvedSupplierId,
    //    CancellationToken ct)
    //{
    //    return sender.Send(
    //        new RemoveApprovedSupplierCommand(
    //            id,
    //            productActiveIngredientId,
    //            approvedSupplierId),
    //        ct);
    //}


    // =========================================================
    // Product Approved Packs
    // =========================================================

    [HttpPost("{id:guid}/packs")]
    public Task<Result<Created>> AddApprovedPack(
        Guid id,
        ApprovedPackRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddApprovedPackCommand(
                id,
                request.Description,
                request.Approval),
            ct);
    }


    [HttpPut("{id:guid}/packs/{approvedPackId:guid}")]
    public Task<Result<Updated>> UpdateApprovedPack(
        Guid id,
        Guid approvedPackId,
        ApprovedPackRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateApprovedPackCommand(
                id,
                approvedPackId,
                request.Description,
                request.Approval),
            ct);
    }


    //[HttpDelete("{id:guid}/packs/{approvedPackId:guid}")]
    //public Task<Result<Deleted>> RemoveApprovedPack(
    //    Guid id,
    //    Guid approvedPackId,
    //    CancellationToken ct)
    //{
    //    return sender.Send(
    //        new RemoveApprovedPackCommand(
    //            id,
    //            approvedPackId),
    //        ct);
    //}


    // =========================================================
    // Active Ingredient Approved Packs
    // =========================================================

    [HttpPost(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/packs")]
    public Task<Result<Created>>
        AddActiveIngredientApprovedPack(
            Guid id,
            Guid productActiveIngredientId,
            ApprovedPackRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new AddActiveIngredientApprovedPackCommand(
                id,
                productActiveIngredientId,
                request.Description,
                request.Approval),
            ct);
    }


    [HttpPut(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/packs/{approvedPackId:guid}")]
    public Task<Result<Updated>>
        UpdateActiveIngredientApprovedPack(
            Guid id,
            Guid productActiveIngredientId,
            Guid approvedPackId,
            ApprovedPackRequest request,
            CancellationToken ct)
    {
        return sender.Send(
            new UpdateActiveIngredientApprovedPackCommand(
                id,
                productActiveIngredientId,
                approvedPackId,
                request.Description,
                request.Approval),
            ct);
    }


    [HttpDelete(
        "{id:guid}/active-ingredients/{productActiveIngredientId:guid}/packs/{approvedPackId:guid}")]
    public Task<Result<Deleted>>
        RemoveActiveIngredientApprovedPack(
            Guid id,
            Guid productActiveIngredientId,
            Guid approvedPackId,
            CancellationToken ct)
    {
        return sender.Send(
            new RemoveActiveIngredientApprovedPackCommand(
                id,
                productActiveIngredientId,
                approvedPackId),
            ct);
    }


    // =========================================================
    // Approved Manufacturers
    // =========================================================

    [HttpPost("{id:guid}/manufacturers")]
    public Task<Result<Created>> AddApprovedManufacturer(
        Guid id,
        AddApprovedManufacturerRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddApprovedManufacturerCommand(
                id,
                request.ManufacturerName,
                request.Approval),
            ct);
    }


    [HttpPatch(
        "{id:guid}/manufacturers/{approvedManufacturerId:guid}/activate")]
    public Task<Result<Updated>> ActivateApprovedManufacturer(
        Guid id,
        Guid approvedManufacturerId,
        CancellationToken ct)
    {
        return sender.Send(
            new ActivateApprovedManufacturerCommand(
                id,
                approvedManufacturerId),
            ct);
    }


    [HttpPatch(
        "{id:guid}/manufacturers/{approvedManufacturerId:guid}/deactivate")]
    public Task<Result<Updated>> DeactivateApprovedManufacturer(
        Guid id,
        Guid approvedManufacturerId,
        CancellationToken ct)
    {
        return sender.Send(
            new DeactivateApprovedManufacturerCommand(
                id,
                approvedManufacturerId),
            ct);
    }


    [HttpDelete(
        "{id:guid}/manufacturers/{approvedManufacturerId:guid}")]
    public Task<Result<Deleted>> RemoveApprovedManufacturer(
        Guid id,
        Guid approvedManufacturerId,
        CancellationToken ct)
    {
        return sender.Send(
            new RemoveApprovedManufacturerCommand(
                id,
                approvedManufacturerId),
            ct);
    }


    // =========================================================
    // Approved Prices
    // =========================================================

    [HttpPost("{id:guid}/prices")]
    public Task<Result<Created>> AddApprovedPrice(
        Guid id,
        ApprovedPriceRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddApprovedPriceCommand(
                id,
                request.Price,
                request.PricePerDescription,
                request.IssueDate,
                request.ExpiryDate),
            ct);
    }


    [HttpPut("{id:guid}/prices/{approvedPriceId:guid}")]
    public Task<Result<Updated>> UpdateApprovedPrice(
        Guid id,
        Guid approvedPriceId,
        ApprovedPriceRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new UpdateApprovedPriceCommand(
                id,
                approvedPriceId,
                request.Price,
                request.PricePerDescription,
                request.IssueDate,
                request.ExpiryDate),
            ct);
    }


    [HttpDelete("{id:guid}/prices/{approvedPriceId:guid}")]
    public Task<Result<Deleted>> RemoveApprovedPrice(
        Guid id,
        Guid approvedPriceId,
        CancellationToken ct)
    {
        return sender.Send(
            new RemoveApprovedPriceCommand(
                id,
                approvedPriceId),
            ct);
    }


    // =========================================================
    // PV
    // =========================================================

    [HttpPost("{id:guid}/pv")]
    public Task<Result<Created>> AddPV(
        Guid id,
        FulfillmentDeadlineRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddPVCommand(
                id,
                request.Description,
                request.Deadline,
                request.Notes),
            ct);
    }


    [HttpPatch("{id:guid}/pv/fulfill")]
    public Task<Result<Updated>> FulfillPV(
        Guid id,
        FulfillRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new FulfillPVCommand(
                id,
                request.FulfillmentDate),
            ct);
    }


    [HttpPatch("{id:guid}/pv/reset")]
    public Task<Result<Updated>> ResetPV(
        Guid id,
        CancellationToken ct)
    {
        return sender.Send(
            new ResetPVFulfillmentCommand(
                id),
            ct);
    }
    // =========================================================
    // Decree 600
    // =========================================================

    [HttpPost("{id:guid}/decree600")]
    public Task<Result<Created>> AddDecree600(
        Guid id,
        FulfillmentDeadlineRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new AddDecree600Command(
                id,
                request.Description,
                request.Deadline,
                request.Notes),
            ct);
    }

    [HttpPatch("{id:guid}/decree600/fulfill")]
    public Task<Result<Updated>> FulfillDecree600(
        Guid id,
        FulfillRequest request,
        CancellationToken ct)
    {
        return sender.Send(
            new FulfillDecree600Command(
                id,
                request.FulfillmentDate),
            ct);
    }


    [HttpPatch("{id:guid}/decree600/reset")]
    public Task<Result<Updated>> ResetDecree600(
        Guid id,
        CancellationToken ct)
    {
        return sender.Send(
            new ResetDecree600FulfillmentCommand(
                id),
            ct);
    }

    [HttpGet("product/{productId:guid}")]
    public Task<Result<ProductBasicDataResponse>>
    GetByProductId(
        Guid productId,
        CancellationToken ct)
    {
        return sender.Send(
            new GetProductBasicDataByProductIdQuery(
                productId),
            ct);
    }

   


}