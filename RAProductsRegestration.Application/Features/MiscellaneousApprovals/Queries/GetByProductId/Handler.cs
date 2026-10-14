using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;
using RAProductsRegestration.Domain.MiscellaneousApprovals;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovals.Queries.GetByProductId
{
    internal sealed class Handler(
    IRepository<MiscellaneousApproval> repository,
    IRepository<MiscellaneousApprovalFileType> fileTypeRepository,
    IRepository<Department> departmentRepository)
    : IQueryHandler<
        GetMiscellaneousApprovalsByProductIdQuery,
        MiscellaneousApprovalResponse[]>
    {
        public async Task<
            Result<MiscellaneousApprovalResponse[]>>
            Handle(
                GetMiscellaneousApprovalsByProductIdQuery request,
                CancellationToken ct)
        {
            // =====================================================
            // Miscellaneous Approvals
            // =====================================================

            var approvals =
                await repository.WhereAsync(
                    x =>
                        x.ProductId ==
                        request.ProductId,
                    ct);


            if (approvals.Count == 0)
            {
                return Array.Empty<
                    MiscellaneousApprovalResponse>();
            }


            // =====================================================
            // File Types
            // =====================================================

            var fileTypeIds =
                approvals
                    .Select(
                        x => x.FileTypeId)
                    .Distinct()
                    .ToArray();


            var fileTypes =
                await fileTypeRepository.WhereAsync(
                    x =>
                        fileTypeIds.Contains(
                            x.Id),
                    ct);


            // =====================================================
            // Departments used by Requirements
            // =====================================================

            var departmentIds =
                approvals
                    .SelectMany(
                        x =>
                            x.ApprovalRequirements)
                    .Select(
                        x =>
                            x.ResponsibleDepartmentId)
                    .Distinct()
                    .ToArray();


            IReadOnlyList<Department> departments;


            if (departmentIds.Length == 0)
            {
                departments =
                    Array.Empty<Department>();
            }
            else
            {
                departments =
                    await departmentRepository.WhereAsync(
                        x =>
                            departmentIds.Contains(
                                x.Id),
                        ct);
            }


            // =====================================================
            // Map
            // =====================================================

            var response =
                MiscellaneousApprovalMapper.Map(
                    approvals,
                    fileTypes,
                    departments);


            return response;
        }
    }
}
