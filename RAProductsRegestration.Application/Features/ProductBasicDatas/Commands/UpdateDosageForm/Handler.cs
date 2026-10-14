using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateDosageForm
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<DosageForm> dosageFormRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateDosageFormCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateDosageFormCommand request,
            CancellationToken ct)
        {
            if (request.DosageFormId.HasValue)
            {
                var exists =
                    await dosageFormRepository.AnyAsync(
                        x =>
                            x.Id == request.DosageFormId &&
                            x.IsActive,
                        ct);

                if (!exists)
                {
                    return ProductBasicDataApplicationErrors
                        .DosageFormNotFound(
                            request.DosageFormId.Value);
                }
            }

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.UpdateDosageForm(
                        request.DosageFormId),
                    ct);
        }
    }
}
