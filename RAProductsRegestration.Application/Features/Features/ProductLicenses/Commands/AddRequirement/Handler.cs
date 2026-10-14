using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.ProductLicenses;

namespace RAProductsRegestration.Application.Features.Features.ProductLicenses.Commands.AddRequirement
{
    internal sealed class Handler(
    IProductLicenseRepository licenseRepository,
    IRepository<Department> departmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<
        AddLicenseRequirementCommand,
        Created>
    {
        public async Task<Result<Created>> Handle(
            AddLicenseRequirementCommand request,
            CancellationToken ct)
        {
            // =====================================================
            // Product License
            // =====================================================

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


            // =====================================================
            // Responsible Department
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
            // Optional Requirement Attachment
            // =====================================================

            var requirementAttachment =
                request.RequirementAttachment?
                    .ToDomain();


            // =====================================================
            // Domain
            // =====================================================

            var result =
                license.AddRequirement(
                    request.Name,
                    request.Deadline,
                    request.ResponsibleDepartmentId,
                    requirementAttachment,
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
