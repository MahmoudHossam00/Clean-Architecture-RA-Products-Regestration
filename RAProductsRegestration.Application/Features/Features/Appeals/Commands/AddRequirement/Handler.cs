using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Commands.AddRequirement
{
    internal sealed class Handler(
    IAppealRepository appealRepository,
    IRepository<Department> departmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddAppealRequirementCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddAppealRequirementCommand request,
            CancellationToken ct)
        {
            var appeal =
                await appealRepository
                    .GetByIdWithRequirementsAsync(
                        request.AppealId,
                        ct);


            if (appeal is null)
            {
                return AppealErrors.NotFound(
                    request.AppealId);
            }


            var departmentExists =
                await departmentRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.ResponsibleDepartmentId,
                    ct);


            if (!departmentExists)
            {
                return DepartmentErrors.NotFound(
                    request.ResponsibleDepartmentId);
            }


            var attachment =
                request.RequirementAttachment?
                    .ToDomain();


            var result =
                appeal.AddRequirement(
                    request.Name,
                    request.Deadline,
                    request.ResponsibleDepartmentId,
                    attachment,
                    request.Notes);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Created;
        }
    }
}
