using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Communications
{
    public interface IEmailService
    {
        Task<Result<Success>> SendAsync(
            EmailMessage message,
            CancellationToken cancellationToken = default);
    }
}
