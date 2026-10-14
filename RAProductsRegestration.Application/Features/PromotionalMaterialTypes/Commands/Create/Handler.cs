using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;

namespace RAProductsRegestration.Application.Features.PromotionalMaterialTypes.Commands.Create
{
    internal sealed class Handler(
    IRepository<PromotionalMaterialType> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreatePromotionalMaterialTypeCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreatePromotionalMaterialTypeCommand request,
            CancellationToken ct)
        {
            var name =
                request.Name.Trim();


            var exists =
                await repository.AnyAsync(
                    x => x.Name == name,
                    ct);


            if (exists)
            {
                return PromotionalMaterialTypeErrors
                    .AlreadyExists(
                        name);
            }


            var result =
                PromotionalMaterialType.Create(
                    name);


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
