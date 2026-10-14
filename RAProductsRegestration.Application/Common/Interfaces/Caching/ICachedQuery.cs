using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Caching
{
    public interface ICachedQuery
    {
        string CacheKey { get; }
        string[] Tags { get; }
        TimeSpan Expiration { get; }
    }

    public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery;
}
