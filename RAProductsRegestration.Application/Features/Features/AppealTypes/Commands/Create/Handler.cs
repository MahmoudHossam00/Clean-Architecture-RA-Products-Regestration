using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.AppealTypes.Commands.Create
{
    internal sealed class Handler(
    IRepository<AppealType> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateAppealTypeCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateAppealTypeCommand request,
            CancellationToken ct)
        {
            var name =
                request.Name.Trim();


            var alreadyExists =
                await repository.AnyAsync(
                    x =>
                        x.Name == name,
                    ct);


            if (alreadyExists)
            {
                return AppealTypeErrors
                    .AlreadyExists(
                        name);
            }


            var result =
                AppealType.Create(
                    name);


            if (result.IsError)
            {
                return result.TopError!;
            }


            repository.Add(
                result.Value);


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Created;
        }
    }
}
