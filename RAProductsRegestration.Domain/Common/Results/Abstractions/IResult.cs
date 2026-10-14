using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Common.Results.Abstractions
{
    public interface IResult
    {
        IReadOnlyList<Error>? Errors { get; }
        bool IsSuccess { get; }
        object? ValueObject { get; }
        Error? TopError { get; }
        bool IsError { get; }
    }

    public interface IResult<out TValue> : IResult
    {
        TValue Value { get; }
    }
}
