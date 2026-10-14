using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.ProductsTechnicalDocs.Commands.AddApprovedMoaValidation
{
    public sealed record AddApprovedMoaValidationCommand(
        Guid ProductTechnicalDocsId,
        DateOnly ApprovalDate,
        AttachmentReferenceInput Attachment)
        : ICommand<Created>;
}
