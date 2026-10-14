using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.Decree600.AddDecree600
{
    public sealed record AddDecree600Command(
        Guid ProductBasicDataId,
        string Description,
        DateOnly Deadline,
        string? Notes)
        : ICommand<Created>;
}
