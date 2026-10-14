using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;

namespace RAProductsRegestration.Application.Features.DosageForms.Commands.Create;

internal sealed class Handler(
    IRepository<DosageForm> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateDosageFormCommand,
        Created>
{
    public async Task<Result<Created>> Handle(
        CreateDosageFormCommand request,
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
                "DosageForm.Name.AlreadyExists",
                $"Dosage form '{normalizedName}' already exists.");
        }

        var createResult =
            DosageForm.Create(
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