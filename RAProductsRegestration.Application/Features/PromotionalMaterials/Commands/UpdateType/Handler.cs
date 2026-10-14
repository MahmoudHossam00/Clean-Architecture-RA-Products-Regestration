using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.PromotionalMaterials;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Commands.UpdateType
{
    internal sealed class Handler(
    IRepository<PromotionalMaterial> repository,
    IRepository<PromotionalMaterialType> typeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdatePromotionalMaterialTypeCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdatePromotionalMaterialTypeCommand request,
            CancellationToken ct)
        {
            var material =
                await repository.GetByIdAsync(
                    request.PromotionalMaterialId,
                    ct);


            if (material is null)
            {
                return PromotionalMaterialErrors
                    .NotFound(
                        request.PromotionalMaterialId);
            }


            var typeExists =
                await typeRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.PromotionalMaterialTypeId,
                    ct);


            if (!typeExists)
            {
                return PromotionalMaterialTypeErrors
                    .NotFound(
                        request.PromotionalMaterialTypeId);
            }


            var result =
                material.UpdateType(
                    request.PromotionalMaterialTypeId);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Updated;
        }
    }
}
