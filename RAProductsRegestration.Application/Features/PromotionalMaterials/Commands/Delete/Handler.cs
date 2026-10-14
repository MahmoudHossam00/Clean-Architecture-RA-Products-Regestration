using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.PromotionalMaterials;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Commands.Delete
{
    internal sealed class Handler(
    IRepository<PromotionalMaterial> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        DeletePromotionalMaterialCommand,
        Deleted>
    {
        public async Task<Result<Deleted>> Handle(
            DeletePromotionalMaterialCommand request,
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


            repository.Remove(
                material);


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Deleted;
        }
    }
}
