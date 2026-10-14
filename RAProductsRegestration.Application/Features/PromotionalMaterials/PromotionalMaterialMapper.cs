using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.PromotionalMaterials;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;
using LookupResponse = RAProductsRegestration.Application.Features.ProductBasicDatas.Queries.GetByProductId.LookupResponse;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials;

internal static class PromotionalMaterialMapper
{
    public static IReadOnlyCollection<PromotionalMaterialResponse>
        Map(
            IReadOnlyCollection<PromotionalMaterial> materials,
            IReadOnlyCollection<PromotionalMaterialType> types)
    {
        var typeMap =
            types.ToDictionary(
                x => x.Id);


        return materials
            .Select(
                material =>
                    Map(
                        material,
                        typeMap))
            .ToArray();
    }


    private static PromotionalMaterialResponse Map(
        PromotionalMaterial material,
        IReadOnlyDictionary<
            Guid,
            PromotionalMaterialType> types)
    {
        if (!types.TryGetValue(
                material.PromotionalMaterialTypeId,
                out var type))
        {
            throw new InvalidOperationException(
                $"Promotional material type '{material.PromotionalMaterialTypeId}' was not found.");
        }


        return new PromotionalMaterialResponse(
            material.Id,

            material.ProductId,

            new LookupResponse(
                type.Id,
                type.Name),

            MapAttachment(
                material.MaterialFile),

            MapDatedAttachment(
                material.Approval),

            material.ApprovalExpiryDate);
    }


    private static AttachmentResponse MapAttachment(
        AttachmentReference attachment)
    {
        return new AttachmentResponse(
            attachment.AttachmentId,
            attachment.FileName,
            attachment.Notes);
    }


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
                attachment.Attachment.AttachmentId,
                attachment.Attachment.FileName,
                attachment.Attachment.Notes));
    }
}