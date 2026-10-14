using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RAProductsRegestration.Domain.Common.Results;
using RAProductsRegestration.Domain.Common.Results.Abstractions;
using IResult = RAProductsRegestration.Domain.Common.Results.Abstractions.IResult;

namespace RAProductsRegestration.API.Filters;

internal sealed class ResultFilter
    : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next)
    {
        if (context.Result is not ObjectResult objectResult ||
            objectResult.Value is not IResult result)
        {
            await next();
            return;
        }

        if (result.IsSuccess)
        {
            context.Result =
                new OkObjectResult(
                    result.ValueObject);

            await next();
            return;
        }

        var topError =
            result.TopError!;

        var statusCode =
            GetStatusCode(
                topError.Type);

        var problemDetails =
            new ProblemDetails
            {
                Status = statusCode,

                Title = GetTitle(
                    topError.Type),

                Detail =
                    topError.Description
            };

        problemDetails.Extensions["errors"] =
            result.Errors
                .Select(error => new
                {
                    error.Code,
                    error.Description
                })
                .ToArray();

        context.Result =
            new ObjectResult(
                problemDetails)
            {
                StatusCode = statusCode,

                ContentTypes =
                {
                    "application/problem+json"
                }
            };

        await next();
    }

    private static int GetStatusCode(
        ErrorKind kind)
    {
        return kind switch
        {
            ErrorKind.Validation =>
                StatusCodes.Status400BadRequest,

            ErrorKind.Failure =>
                StatusCodes.Status400BadRequest,

            ErrorKind.NotFound =>
                StatusCodes.Status404NotFound,

            ErrorKind.Conflict =>
                StatusCodes.Status409Conflict,

            ErrorKind.Unauthorized =>
                StatusCodes.Status401Unauthorized,

            ErrorKind.Forbidden =>
                StatusCodes.Status403Forbidden,

            ErrorKind.Unexpected =>
                StatusCodes.Status500InternalServerError,

            _ =>
                StatusCodes.Status500InternalServerError
        };
    }

    private static string GetTitle(
        ErrorKind kind)
    {
        return kind switch
        {
            ErrorKind.Validation =>
                "Validation failed",

            ErrorKind.Failure =>
                "Request failed",

            ErrorKind.NotFound =>
                "Resource not found",

            ErrorKind.Conflict =>
                "Conflict",

            ErrorKind.Unauthorized =>
                "Unauthorized",

            ErrorKind.Forbidden =>
                "Forbidden",

            ErrorKind.Unexpected =>
                "Unexpected error",

            _ =>
                "Request failed"
        };
    }
}