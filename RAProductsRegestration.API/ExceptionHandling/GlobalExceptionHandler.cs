using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RAProductsRegestration.API.ExceptionHandling;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled exception occurred.");

        httpContext.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,

                Exception = exception,

                ProblemDetails = new ProblemDetails
                {
                    Status =
                        StatusCodes.Status500InternalServerError,

                    Title =
                        "An unexpected error occurred.",

                    Detail =
                        "An unexpected error occurred while " +
                        "processing the request."
                }
            });
    }
}