using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.AddApprovedPrice;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.UpdateApprovedPrice
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<DosageForm> dosageFormRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateApprovedPriceCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateApprovedPriceCommand request,
            CancellationToken ct)
        {


            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                   x => x.UpdateApprovedPrice(
                    request.ApprovedPriceId,
                    request.Price,
                    request.PricePerDescription,
                    request.IssueDate,
                    request.ExpiryDate),
                    ct);
        }
    }
}
