using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.LicenseCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.LicenseCategories.Commands.Create
{
    internal sealed class Handler(
    IRepository<LicenseCategory> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateLicenseCategoryCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateLicenseCategoryCommand request,
            CancellationToken ct)
        {
            var name =
                request.Name.Trim();


            var alreadyExists =
                await repository.AnyAsync(
                    x => x.Name == name,
                    ct);


            if (alreadyExists)
            {
                return LicenseCategoryErrors
                    .AlreadyExists(name);
            }


            var createResult =
                LicenseCategory.Create(
                    name);


            if (createResult.IsError)
            {
                return createResult.TopError!;
            }


            repository.Add(
                createResult.Value);


            await unitOfWork
                .SaveChangesAsync(ct);


            return Result.Created;
        }
    }
}
