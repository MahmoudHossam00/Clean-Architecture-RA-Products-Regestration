using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.DosageForms.Queries.GetAll
{
    public sealed record DosageFormLookupResponse(
    Guid Id,
    string Name);
}
