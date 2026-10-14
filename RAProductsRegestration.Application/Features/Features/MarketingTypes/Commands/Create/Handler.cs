using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MarketingTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.Features.MarketingTypes.Commands.Create
{
    internal sealed class Handler(
    IRepository<MarketingType> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateMarketingTypeCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateMarketingTypeCommand request,
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
                return MarketingTypeErrors
                    .AlreadyExists(name);
            }


            var createResult =
                MarketingType.Create(
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
