using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Domain.Common.Results
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorKind Type { get; }
        private Error(string code, string description,ErrorKind type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public static Error Failure(string code = nameof(Failure), string description = "General Failure")
            => new Error(code, description, ErrorKind.Failure);

        public static Error UnExpected(string Code = nameof(UnExpected), string Description = "Unexpected Error")
        => new Error(Code, Description, ErrorKind.Unexpected);
        public static Error Validation(string Code = nameof(Validation), string Description = "Validation Error")
        => new Error(Code, Description, ErrorKind.Validation);
        public static Error Conflict(string Code = nameof(Conflict), string Description = "Conflict Error")
        => new Error(Code, Description, ErrorKind.Conflict);
        public static Error NotFound(string Code = nameof(NotFound), string Description = "Not Found Error")
        => new Error(Code, Description, ErrorKind.NotFound);
        public static Error UnAuthorized(string Code = nameof(UnAuthorized), string Description = "Unauthorized")
        => new Error(Code, Description, ErrorKind.Unauthorized);
        public static Error Forbidden(string Code = nameof(Forbidden), string Description = "Forbidden Error")
        => new Error(Code, Description, ErrorKind.Forbidden);
        public static Error GeneralError(ErrorKind type, string Code, string Description)
        => new Error(Code, Description, type);
    }
}
