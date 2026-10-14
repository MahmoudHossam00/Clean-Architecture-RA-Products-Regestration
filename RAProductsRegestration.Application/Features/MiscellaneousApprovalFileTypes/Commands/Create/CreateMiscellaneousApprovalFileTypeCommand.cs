using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Features.MiscellaneousApprovalFileTypes.Commands.Create
{
    public sealed record CreateMiscellaneousApprovalFileTypeCommand(
        string Name)
        : ICommand<Created>;
}
