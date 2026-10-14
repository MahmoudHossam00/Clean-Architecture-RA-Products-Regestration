using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.UpdateActiveIngredientStrength;
using RAProductsRegestration.Application.Features.ProductBasicDatas.Common;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductBasicDatas;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.ActiveIngredients.Commands.AddApprovedSupplier
{
    internal sealed class Handler(
    IProductBasicDataRepository repository,
    IRepository<ProductBasicData> productbasicdataRepository,
    IRepository<Supplier> supplierRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddApprovedSupplierCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddApprovedSupplierCommand request,
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
            var supplierExists =
            await supplierRepository.AnyAsync(
            x => x.Id == request.SupplierId,
            ct);

            if (!supplierExists)
            {
                return ProductBasicDataApplicationErrors
                    .SupplierNotFound(
                        request.SupplierId);
            }
            return await ProductBasicDataCommandExecutor
                .ExecuteAsync(
                    request.ProductBasicDataId,
                    repository,
                    unitOfWork,
                    x => x.AddApprovedSupplier(
             request.ProductActiveIngredientId,
             request.SupplierId),
                    ct);
        }
    }
}
