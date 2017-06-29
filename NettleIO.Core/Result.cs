using System;

namespace NettleIO.Core
{
    public class Result<TValue> : Result, IValueResult<TValue>
    {
        protected Result()
        {
        }

        internal Result(TValue value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value),
                    "Result value was null. Use Result.Empty<T>(), Result.Fail<T>() or Result.Success<T>() instead.");

            Value = value;
        }

        public virtual bool IsEmpty => Value == null;
        public virtual bool HasValue => Value != null;
        public virtual TValue Value { get; }

        public new static Result<TValue> Fail(string message = "")
        {
            return new Result<TValue> {Succeeded = false, Message = message};
        }

        public new static Result<TValue> Fail(Exception ex, string message = "")
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            return new Result<TValue> {Succeeded = false, Error = ex, Message = message};
        }

        public new static Result<TValue> Success(string message = "")
        {
            return new Result<TValue> {Message = message};
        }

        public static Result<TValue> SuccessWithValue(TValue value, string message = "")
        {
            return new Result<TValue>(value) {Succeeded = true, Message = message};
        }
    }

    public class Result : IActionResult
    {
        protected Result()
        {
        }

        protected Result(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

            Message = message;
        }

        public virtual bool Failed => !Succeeded;

        public virtual bool Succeeded { get; protected set; }

        public virtual Exception Error { get; protected set; }
        public virtual IActionMetric Metrics { get; protected set; }
        public virtual string Message { get; protected set; }

        public static Result Fail(string message = "")
        {
            return new Result(message) {Succeeded = false};
        }

        public static Result Success(object value, string message = "")
        {
            return new Result {Succeeded = true};
        }

        public static Result Fail(Exception ex, string message = "")
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            return new Result(message) {Succeeded = false, Error = ex};
        }

        public static Result<TValue> SuccessWithValue<TValue>(TValue value, string message = "")
        {
            return Result<TValue>.SuccessWithValue(value, message);
        }

        public static Result<TValue> Success<TValue>(string message = "")
        {
            return Result<TValue>.Success(message);
        }

        public override string ToString()
        {
            var str = string.Empty;
            if (Succeeded)
            {
                str += "Success";
            }
            else if (Error == null)
            {
                str += "Failed";
            }
            else
            {
                str += "Failed with an error";
            }

            if (!string.IsNullOrEmpty(Message))
            {
                str += $": {Message}";
            }

            if (Error != null)
            {
                str += $" ({Error.Message})";
            }

            return str;
        }
    }
}