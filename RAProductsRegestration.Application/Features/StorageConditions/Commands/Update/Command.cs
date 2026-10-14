using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.StorageConditions.Commands.Update
{
    public sealed record UpdateStorageConditionCommand(
    Guid Id,
    string Name)
    : ICommand<Updated>;
}
