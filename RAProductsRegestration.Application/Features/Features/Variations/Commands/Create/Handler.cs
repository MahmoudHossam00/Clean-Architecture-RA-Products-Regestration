using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.Create
{
    internal sealed class Handler(
    IVariationRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateVariationCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateVariationCommand request,
            CancellationToken ct)
        {
            var result =
                Variation.Create(
                    request.ProductId,
                    request.Initiator,
                    request.Cause,
                    request.Deadline,
                    request.DeadlineNote);


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
