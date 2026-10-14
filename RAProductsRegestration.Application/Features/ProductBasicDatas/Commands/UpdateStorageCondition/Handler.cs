using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.StorageConditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateStorageCondition
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<StorageCondition> dosageFormRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateStorageConditionCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateStorageConditionCommand request,
            CancellationToken ct)
        {
            if (request.StorageConditionId.HasValue)
            {
                var exists =
                    await dosageFormRepository.AnyAsync(
                        x =>
                            x.Id == request.StorageConditionId &&
                            x.IsActive,
                        ct);

                if (!exists)
                {
                    return ProductBasicDataApplicationErrors
                        .StorageConditionNotFound(
                            request.StorageConditionId.Value);
                }
            }

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.UpdateStorageCondition(
                        request.StorageConditionId),
                    ct);
        }
    }
}
