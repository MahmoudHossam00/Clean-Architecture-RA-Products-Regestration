
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductsTechnicalDocs;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.CreateProductTechnicalDocs
{
    internal sealed class Handler(
     IRepository<ProductTechnicalDocs> repository,
     IUnitOfWork unitOfWork)
     : ICommandHandler<
         CreateProductTechnicalDocsCommand,
         Created>
    {
        public async Task<Result<Created>> Handle(
            CreateProductTechnicalDocsCommand request,
            CancellationToken ct)
        {
            var alreadyExists =
                await repository.AnyAsync(
                    x => x.ProductId == request.ProductId,
                    ct);

            if (alreadyExists)
            {
                return ProductTechnicalDocsErrors
                    .AlreadyExistsForProduct(
                        request.ProductId);
            }


            var createResult =
                ProductTechnicalDocs.Create(
                    request.ProductId);

            if (createResult.IsError)
            {
                return createResult.TopError!;
            }


            repository.Add(
                createResult.Value);

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Created;
        }
    }
}
