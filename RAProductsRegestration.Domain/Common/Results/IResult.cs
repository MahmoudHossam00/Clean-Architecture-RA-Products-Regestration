using RAProductsRegestration.Domain.Common.Results.Abstractions;

namespace RAProductsRegestration.Domain.Common.Results;

public sealed class Result<TValue>
    : IResult<TValue>,
      IResultFactory<Result<TValue>>
{
    private readonly TValue? _value;

    private readonly List<Error> _errors = [];

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "Cannot access Value when the result is an error.");

    public object? ValueObject =>
        IsSuccess
            ? _value
            : null;

    public IReadOnlyList<Error> Errors =>
        IsError
            ? _errors
            : [];

    public Error? TopError =>
        IsError && _errors.Count > 0
            ? _errors[0]
            : null;

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    private Result(
        TValue value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(
                nameof(value));
        }

        _value = value;
        IsSuccess = true;
    }

    private Result(
        Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        _errors = [error];

        IsSuccess = false;
    }

    private Result(
        List<Error> errors)
    {
        if (errors is null ||
            errors.Count == 0)
        {
            throw new ArgumentException(
                "At least one error is required.",
                nameof(errors));
        }

        _errors = errors;

        IsSuccess = false;
    }

    public static Result<TValue> Failure(
        List<Error> errors)
    {
        return new Result<TValue>(
            errors);
    }

    public TNextValue OnMatch<TNextValue>(
        Func<TValue, TNextValue> onValue,
        Func<IReadOnlyCollection<Error>, TNextValue> onError)
    {
        return IsSuccess
            ? onValue(Value)
            : onError(Errors);
    }

    public static implicit operator Result<TValue>(
        TValue value)
    {
        return new Result<TValue>(
            value);
    }

    public static implicit operator Result<TValue>(
        Error error)
    {
        return new Result<TValue>(
            error);
    }

    public static implicit operator Result<TValue>(
        List<Error> errors)
    {
        return new Result<TValue>(
            errors);
    }
}