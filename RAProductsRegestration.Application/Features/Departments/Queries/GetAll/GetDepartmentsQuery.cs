using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;

namespace RAProductsRegestration.Application.Features.Departments.Queries.GetAll
{
    public sealed record GetDepartmentsQuery(
    string? Search)
    : IQuery<
        IReadOnlyCollection<LookupResponse>>;
}
