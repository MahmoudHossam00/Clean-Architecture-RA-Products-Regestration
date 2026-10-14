using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateStorageCondition
{
    public sealed record UpdateStorageConditionCommand(
     Guid ProductBasicDataId,
     Guid? StorageConditionId)
     : ICommand<Updated>;
}
