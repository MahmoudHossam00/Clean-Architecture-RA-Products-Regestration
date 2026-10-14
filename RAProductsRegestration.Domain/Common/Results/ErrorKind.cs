using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Common.Results
{
    public enum ErrorKind
    {
        Failure,
        Unexpected,
        Validation,
        Conflict,
        NotFound,
        Unauthorized,
        Forbidden
    }
}
