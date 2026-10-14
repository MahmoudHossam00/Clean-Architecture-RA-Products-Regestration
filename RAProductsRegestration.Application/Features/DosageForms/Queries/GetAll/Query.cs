using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.DosageForms.Queries.GetAll
{
    public sealed record GetDosageFormsQuery(
    bool ActiveOnly = true)
    : IQuery<IReadOnlyCollection<DosageFormLookupResponse>>;
}
