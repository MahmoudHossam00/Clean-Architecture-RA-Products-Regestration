using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.PromotionalMaterials;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Commands.Create
{
    internal sealed class Handler(
    IRepository<PromotionalMaterial> repository,
    IRepository<PromotionalMaterialType> typeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreatePromotionalMaterialCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreatePromotionalMaterialCommand request,
            CancellationToken ct)
        {
            // =====================================================
            // Promotional Material Type
            // =====================================================

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


            // =====================================================
            // Material File
            // =====================================================

            var materialFile =
                request.MaterialFile
                    .ToDomain();


            // =====================================================
            // Domain
            // =====================================================

            var result =
                PromotionalMaterial.Create(
                    request.ProductId,
                    request.PromotionalMaterialTypeId,
                    materialFile);


            if (result.IsError)
            {
                return result.TopError!;
            }


            repository.Add(
                result.Value);


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Created;
        }
    }
}
