using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterialTypes.Commands.Create
{
    public sealed record CreatePromotionalMaterialTypeCommand(
    string Name)
    : ICommand<Created>;
}
