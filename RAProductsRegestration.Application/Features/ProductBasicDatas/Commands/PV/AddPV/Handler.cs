using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.PV.AddPV
{
    internal sealed class Handler(
    IRepository<ProductBasicData> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddPVCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddPVCommand request,
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
                basicData.AddPV(
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
