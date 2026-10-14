using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using RAProductsRegestration.Application.Common.Interfaces.Caching;
using RAProductsRegestration.Domain.Common.Results.Abstractions;

namespace RAProductsRegestration.Application.Common.Behaviours;

public sealed class CachingBehaviour<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (request is not ICachedQuery cachedRequest)
        {
            return await next(ct);
        }

        logger.LogInformation(
            "Checking cache for {RequestName}",
            typeof(TRequest).Name);

        var cachedResult =
            await cache.GetOrCreateAsync<TResponse?>(
                cachedRequest.CacheKey,
                _ => ValueTask.FromResult<TResponse?>(null),
                new HybridCacheEntryOptions
                {
                    Flags =
                        HybridCacheEntryFlags.DisableUnderlyingData
                },
                cancellationToken: ct);

        if (cachedResult is not null)
        {
            return cachedResult;
        }

        var result = await next(ct);

        if (result is IResult response &&
            response.IsSuccess)
        {
            logger.LogInformation(
                "Caching result for {RequestName}",
                typeof(TRequest).Name);

            await cache.SetAsync(
                cachedRequest.CacheKey,
                result,
                new HybridCacheEntryOptions
                {
                    Expiration =
                        cachedRequest.Expiration
                },
                cachedRequest.Tags,
                ct);
        }

        return result;
    }
}