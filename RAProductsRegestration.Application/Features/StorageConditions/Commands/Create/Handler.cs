using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;

using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;
using RAProductsRegestration.Domain.StorageConditions;

namespace RAProductsRegestration.Application.Features.StorageConditions.Commands.Create;

internal sealed class Handler(
    IRepository<StorageCondition> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateStorageConditionCommand,
        Created>
{
    public async Task<Result<Created>> Handle(
        CreateStorageConditionCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedName =
            request.Name.Trim();

        var exists =
            await repository.AnyAsync(
                x => x.Name == normalizedName,
                cancellationToken);

        if (exists)
        {
            return Error.Conflict(
                "StorageCondition.Name.AlreadyExists",
                $"Storage Condition '{normalizedName}' already exists.");
        }

        var createResult =
            StorageCondition.Create(
                normalizedName);

        if (createResult.IsError)
        {
            return createResult.TopError!;
        }

        repository.Add(
            createResult.Value);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Created;
    }
}