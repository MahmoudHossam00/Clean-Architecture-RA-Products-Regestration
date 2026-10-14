using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductBasicDatas.Commands.UpdateDosageForm
{
    public sealed record UpdateDosageFormCommand(
    Guid ProductBasicDataId,
    Guid? DosageFormId)
    : ICommand<Updated>;
}
