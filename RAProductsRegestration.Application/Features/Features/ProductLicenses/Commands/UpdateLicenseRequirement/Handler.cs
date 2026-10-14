using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.UpdateLicenseRequirement
{
    internal sealed class Handler(
    IProductLicenseRepository licenseRepository,
    IRepository<Department> departmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        UpdateLicenseRequirementCommand,
        Updated>
    {
        public async Task<Result<Updated>> Handle(
            UpdateLicenseRequirementCommand request,
            CancellationToken ct)
        {
            var license =
                await licenseRepository
                    .GetByIdWithRequirementsAsync(
                        request.ProductLicenseId,
                        ct);


            if (license is null)
            {
                return ProductLicenseErrors
                    .NotFound(
                        request.ProductLicenseId);
            }


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


            var result =
                license.UpdateRequirement(
                    request.RequirementId,
                    request.Name,
                    request.Deadline,
                    request.ResponsibleDepartmentId,
                    request.Notes);


            if (result.IsError)
            {
                return result.TopError!;
            }


            await unitOfWork
                .SaveChangesAsync(
                    ct);


            return Result.Updated;
        }
    }

}
