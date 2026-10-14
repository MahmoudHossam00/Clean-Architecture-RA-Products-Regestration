using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Commands.Requirements.Add
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IRepository<Department> departmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddApprovalRequirementCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddApprovalRequirementCommand request,
            CancellationToken ct)
        {
            // =====================================================
            // Miscellaneous Approval
            // =====================================================

            var approval =
                await repository.GetByIdAsync(
                    request.MiscellaneousApprovalId,
                    ct);


            if (approval is null)
            {
                return MiscellaneousApprovalErrors
                    .NotFound(
                        request.MiscellaneousApprovalId);
            }


            // =====================================================
            // Department
            // =====================================================

            var departmentExists =
                await departmentRepository.AnyAsync(
                    x =>
                        x.Id ==
                        request.ResponsibleDepartmentId,
                    ct);


            if (!departmentExists)
            {
                return DepartmentErrors
                    .NotFound(
                        request.ResponsibleDepartmentId);
            }


            // =====================================================
            // Optional Attachment
            // =====================================================

            var attachment =
                request.RequirementAttachment?
                    .ToDomain();


            // =====================================================
            // Domain
            // =====================================================

            var result =
                approval.AddApprovalRequirement(
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
