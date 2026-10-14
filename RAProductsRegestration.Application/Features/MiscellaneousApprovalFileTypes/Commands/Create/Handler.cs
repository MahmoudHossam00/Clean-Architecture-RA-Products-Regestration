using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovalFileTypes.Commands.Create
{
    internal sealed class Handler(
    IRepository<MiscellaneousApprovalFileType> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        CreateMiscellaneousApprovalFileTypeCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            CreateMiscellaneousApprovalFileTypeCommand request,
            CancellationToken ct)
        {
            var name =
                request.Name.Trim();


            var exists =
                await repository.AnyAsync(
                    x => x.Name == name,
                    ct);


            if (exists)
            {
                return MiscellaneousApprovalFileTypeErrors
                    .AlreadyExists(
                        name);
            }


            var result =
                MiscellaneousApprovalFileType.Create(
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
