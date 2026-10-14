using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Common.Results.Abstractions
{
    public interface IResultFactory<TSelf>
    where TSelf : IResult
    {
        static abstract TSelf Failure(List<Error> errors);
    }
}
