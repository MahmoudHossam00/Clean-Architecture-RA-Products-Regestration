using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.AddDecree600
{
    internal sealed class Handler(
    IRepository<ProductBasicData> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddDecree600Command,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddDecree600Command request,
            CancellationToken ct)
        {
            var basicData =
                await repository.GetByIdAsync(
                    request.ProductBasicDataId,
                    ct);


            if (basicData is null)
            {
                return ProductBasicDataErrors.NotFound(
                    request.ProductBasicDataId);
            }


            var result =
                basicData.AddDecree600(
                    request.Description,
                    request.Deadline,
                    request.Notes);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork.SaveChangesAsync(
                ct);


            return Result.Created;
        }
    }
}
