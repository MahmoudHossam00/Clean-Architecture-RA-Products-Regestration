using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.StorageConditions.Commands.Create
{
    public sealed record CreateStorageConditionCommand(
    string Name)
    : ICommand<Created>;
}
