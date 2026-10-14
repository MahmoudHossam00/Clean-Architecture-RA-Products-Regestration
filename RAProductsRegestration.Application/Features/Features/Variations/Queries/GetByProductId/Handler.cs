using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Departments;

namespace RAProductsRegestration.Application.Features
    .Features.Variations.Queries.GetByProductId;

internal sealed class Handler(
    IVariationRepository variationRepository,
    IRepository<Department> departmentRepository)
    : IQueryHandler<
        GetVariationsByProductIdQuery,
        VariationResponse[]>
{
    public async Task<Result<VariationResponse[]>> Handle(
        GetVariationsByProductIdQuery request,
        CancellationToken ct)
    {
        var variations =
            await variationRepository
                .GetByProductIdWithRequirementsAsync(
                    request.ProductId,
                    ct);


        var departmentIds =
            variations
                .SelectMany(
                    x => x.Requirements)
                .Select(
                    x => x.ResponsibleDepartmentId)
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
                await departmentRepository
                    .WhereAsync(
                        x =>
                            departmentIds.Contains(
                                x.Id),
                        ct);
        }


        return VariationMapper.Map(
            variations,
            departments);
    }
}