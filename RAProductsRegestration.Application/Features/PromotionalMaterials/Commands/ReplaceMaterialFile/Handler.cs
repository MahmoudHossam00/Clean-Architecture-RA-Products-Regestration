using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.PromotionalMaterials;

namespace RAProductsRegestration.Application.Features
    .PromotionalMaterials.Commands.ReplaceMaterialFile;

internal sealed class Handler(
    IRepository<PromotionalMaterial> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        ReplacePromotionalMaterialFileCommand,
        Updated>
{
    public async Task<Result<Updated>> Handle(
        ReplacePromotionalMaterialFileCommand request,
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


        var file =
            request.MaterialFile
                .ToDomain();


        var result =
            material.ReplaceMaterialFile(
                file);


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