using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.DosageForms.Commands.Update
{
    internal sealed class Handler(
    IRepository<DosageForm> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateDosageFormCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateDosageFormCommand request,
            CancellationToken cancellationToken)
        {
            var dosageForm =
                await repository.GetByIdAsync(
                    request.Id,
                    cancellationToken);

            if (dosageForm is null)
            {
                return Error.NotFound(
                    "DosageForm.NotFound",
                    $"Dosage form '{request.Id}' was not found.");
            }

            var duplicate =
                await repository.AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.Name == request.Name.Trim(),
                    cancellationToken);

            if (duplicate)
            {
                return Error.Conflict(
                    "DosageForm.Name.AlreadyExists",
                    $"Dosage form '{request.Name}' already exists.");
            }

            var result =
                dosageForm.UpdateName(
                    request.Name);

            if (result.IsError)
            {
                return result.TopError!;
            }

            await unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result.Updated;
        }
    }
}
