using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.Variations;

namespace RAProductsRegestration.Application.Features.Features.Variations.Commands.UpdateRequirement
{
    internal sealed class Handler(
    IVariationRepository variationRepository,
    IRepository<Department> departmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateVariationRequirementCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateVariationRequirementCommand request,
            CancellationToken ct)
        {
            var variation =
                await variationRepository
                    .GetByIdWithRequirementsAsync(
                        request.VariationId,
                        ct);


            if (variation is null)
            {
                return VariationErrors.NotFound(
                    request.VariationId);
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


            var result =
                variation.UpdateRequirement(
                    request.RequirementId,
                    request.Name,
                    request.Deadline,
                    request.ResponsibleDepartmentId,
                    request.Notes);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork.SaveChangesAsync(
                ct);


            return Result.Updated;
        }
    }
}
