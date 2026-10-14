using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;

namespace RAProductsRegestration.Application.Features.Features.Appeals.Queries.GetByProductId
{
    internal sealed class Handler(
    IAppealRepository appealRepository,
    IRepository<AppealType> appealTypeRepository,
    IRepository<Department> departmentRepository)
    : IQueryHandler<
        GetAppealsByProductIdQuery,
        AppealResponse[]>
    {
        public async Task<Result<AppealResponse[]>> Handle(
            GetAppealsByProductIdQuery request,
            CancellationToken ct)
        {
            // =====================================================
            // Appeals + Requirements
            // =====================================================

            var appeals =
                await appealRepository
                    .GetByProductIdWithRequirementsAsync(
                        request.ProductId,
                        ct);


            if (appeals.Count == 0)
            {
                return Array.Empty<AppealResponse>();
            }


            // =====================================================
            // Appeal Types
            // =====================================================

            var appealTypeIds =
                appeals
                    .Select(
                        x =>
                            x.AppealTypeId)
                    .Distinct()
                    .ToArray();


            var appealTypes =
                await appealTypeRepository
                    .WhereAsync(
                        x =>
                            appealTypeIds.Contains(
                                x.Id),
                        ct);


            // =====================================================
            // Departments
            // =====================================================

            var departmentIds =
                appeals
                    .SelectMany(
                        x => x.Requirements)
                    .Select(
                        x =>
                            x.ResponsibleDepartmentId)
                    .Distinct()
                    .ToArray();


            IReadOnlyList<Department>
                departments;


            if (departmentIds.Length == 0)
            {
                departments =
                    Array.Empty<Department>();
            }
            else
            {
                departments =
                    await departmentRepository
                        .WhereAsync(
                            x =>
                                departmentIds.Contains(
                                    x.Id),
                            ct);
            }


            // =====================================================
            // Map
            // =====================================================

            return AppealMapper.Map(
                appeals,
                appealTypes,
                departments);
        }
    }
}
