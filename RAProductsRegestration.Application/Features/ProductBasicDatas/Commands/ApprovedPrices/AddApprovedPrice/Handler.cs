using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.DosageForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ApprovedPrices.AddApprovedPrice
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<DosageForm> dosageFormRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddApprovedPriceCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddApprovedPriceCommand request,
            CancellationToken ct)
        {
            

            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.AddApprovedPrice(
                    request.Price,
                    request.pricePerDescription,
                    request.IssueDate,
                    request.ExpiryDate),
                    ct);
        }
    }
}
