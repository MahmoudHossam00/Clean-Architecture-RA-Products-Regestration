using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.PromotionalMaterials;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;

namespace RAProductsRegestration.Application.Features
    .PromotionalMaterials.Queries.GetByProductId;

internal sealed class Handler(
    IRepository<PromotionalMaterial> repository,
    IRepository<PromotionalMaterialType> typeRepository)
    : IQueryHandler<
        GetPromotionalMaterialsByProductIdQuery,
        PromotionalMaterialResponse[]>
{
    public async Task<
        Result<PromotionalMaterialResponse[]>>
        Handle(
            GetPromotionalMaterialsByProductIdQuery request,
            CancellationToken ct)
    {
        // =====================================================
        // Promotional Materials
        // =====================================================

        var materials =
            await repository.WhereAsync(
                x =>
                    x.ProductId ==
                    request.ProductId,
                ct);


        // =====================================================
        // No Materials
        // =====================================================

        if (materials.Count == 0)
        {
            return Array.Empty<
                PromotionalMaterialResponse>();
        }


        // =====================================================
        // Required Promotional Material Types
        // =====================================================

        var typeIds =
            materials
                .Select(
                    x =>
                        x.PromotionalMaterialTypeId)
                .Distinct()
                .ToArray();


        var types =
            await typeRepository.WhereAsync(
                x =>
                    typeIds.Contains(
                        x.Id),
                ct);


        // =====================================================
        // Map
        // =====================================================

        var response =
            PromotionalMaterialMapper
                .Map(
                    materials,
                    types)
                .ToArray();


        return response;
    }
}