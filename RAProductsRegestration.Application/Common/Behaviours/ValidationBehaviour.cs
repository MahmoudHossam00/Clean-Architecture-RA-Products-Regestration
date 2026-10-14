using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using RAProductsRegestration.Domain.Common.Results.Abstractions;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Common.Behaviours
{
    public sealed class ValidationBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse :
        IResult,
        IResultFactory<TResponse>
    {
        private readonly IReadOnlyCollection<IValidator<TRequest>> _validators =
            validators.ToArray();

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Count == 0)
            {
                return await next(cancellationToken);
            }

            var context =
                new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(
                    _validators.Select(x =>
                        x.ValidateAsync(
                            context,
                            cancellationToken)));

            var errors = validationResults
                .SelectMany(x => x.Errors)
                .Where(x => x is not null)
                .Select(x =>
                    Error.Validation(
                        Code: x.ErrorCode,
                        Description: x.ErrorMessage))
                .ToList();

            if (errors.Count == 0)
            {
                return await next(cancellationToken);
            }

            return TResponse.Failure(errors);
        }
    }
}
