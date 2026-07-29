using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public record Error(string code , string description, ErrorType type = ErrorType.Failure)
    {
   
        public static Error Failure(string code = "General.Failure", string description = "General Failure Occured")
        {
            return new Error(code, description);
        }
        public static Error Validation(string code = "General.Validation", string description = "General Validation Occured" , ErrorType type = ErrorType.Validation)
        {
            return new Error(code, description);
        }
        public static Error NotFound(string code = "General.NotFound", string description = "Resource Is Not Found", ErrorType type = ErrorType.NotFound)
        {
            return new Error(code, description);
        }
        public static Error Conflict(string code = "General.Conflict", string description = "General Conflict Occured", ErrorType type = ErrorType.Conflict)
        {
            return new Error(code, description);
        }
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "Access Denied, Unauthorized User", ErrorType type = ErrorType.Unauthorized)
        {
            return new Error(code, description);
        }
        public static Error Forbidden(string code = "General.Forbidden", string description = "Forbidden Access, Unauthorized User", ErrorType type = ErrorType.Forbidden)
        {
            return new Error(code, description);
        }
        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "General InvalidCredentials Occured", ErrorType type = ErrorType.InvalidCredintial)
        {
            return new Error(code, description);
        }
    }

    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        InvalidCredintial = 6,
    }
}
