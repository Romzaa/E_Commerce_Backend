using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public class Result
    {
        public bool _isSuccess { get; }
        public IReadOnlyList<Error> _errors { get; }


        protected Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            _isSuccess = isSuccess;
            _errors = errors;
        }

        public static Result Ok() 
        {
            return new Result(true, Array.Empty<Error>());
        }
        public static Result Fail(Error error) 
        {
            return new Result(false, new[] {error});
        }
        public static Result Fail(IReadOnlyList<Error> errors) 
        {
            return new Result(false, errors);
        }
    }
    public class Result<T> : Result
    {
        private readonly T _value = default!;
        public T data => _isSuccess ? _value : throw new InvalidOperationException("Can't Access Value Of Failed Result");

        private Result(T value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }
        private Result(Error error) : base(false, new[] {error})
        {
        }
        private Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
        }

        public static Result<T> Ok(T value) 
        {
            return new Result<T>(value);   
        }

        public static new Result<T> Fail( Error error)
        {
            return new Result<T>(error);
        }
        public static new Result<T> Fail(IReadOnlyList<Error> errors)
        {
            return new Result<T>(errors);
        }
    }
}
