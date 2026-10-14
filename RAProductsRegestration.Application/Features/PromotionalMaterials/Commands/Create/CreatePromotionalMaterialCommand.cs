using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.PromotionalMaterials.Commands.Create
{
    public sealed record CreatePromotionalMaterialCommand(
      Guid ProductId,
      Guid PromotionalMaterialTypeId,
      AttachmentReferenceInput MaterialFile)
      : ICommand<Created>;
}
