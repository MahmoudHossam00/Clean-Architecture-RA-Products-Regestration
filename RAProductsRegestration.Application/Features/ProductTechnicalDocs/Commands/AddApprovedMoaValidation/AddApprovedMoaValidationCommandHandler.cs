using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.ProductsTechnicalDocs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.AddApprovedMoaValidation
{
    internal sealed class Handler(
    IRepository<ProductTechnicalDocs> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddApprovedMoaValidationCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddApprovedMoaValidationCommand request,
            CancellationToken ct)
        {
            var technicalDocs =
                await repository.GetByIdAsync(
                    request.ProductTechnicalDocsId,
                    ct);

            if (technicalDocs is null)
            {
                return ProductTechnicalDocsErrors.NotFound(
                    request.ProductTechnicalDocsId);
            }


            var result =
                technicalDocs.AddApprovedMoaValidation(
                    request.ApprovalDate,
                    request.Attachment.ToDomain());

            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork.SaveChangesAsync(ct);

            return Result.Created;
        }
    }
}
