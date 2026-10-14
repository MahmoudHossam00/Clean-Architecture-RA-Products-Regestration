using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddApprovedSupplier;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddActiveIngredientApprovedPack
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<ProductBasicData> productbasicdataRepository,
    IRepository<Supplier> supplierRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddActiveIngredientApprovedPackCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddActiveIngredientApprovedPackCommand request,
            CancellationToken ct)
        {
            var exists =
                await productbasicdataRepository.AnyAsync(
                    x => x.Id == request.ProductBasicDataId && x.ProductActiveIngredients.Any(p => p.Id == request.ProductActiveIngredientId),
                    ct);

            if (!exists)
            {
                return ProductBasicDataApplicationErrors
                    .NotFound(
                        request.ProductBasicDataId);
            }
            
            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.AddActiveIngredientApprovedPack(
             request.ProductActiveIngredientId,
             request.Description,request.Approval.ToDomain()),
                    ct);
        }
    }
}
