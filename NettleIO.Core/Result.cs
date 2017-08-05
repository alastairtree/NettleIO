using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public class Result<TValue> : Result, IStageValueResult<TValue>
    {
        internal Result()
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
    }

    public class Result : IStageResult
    {
        protected Result()
        {
        }

        protected Result(string message)
        {
            Message = message;
        }

        public virtual bool Failed => !Succeeded;

        public virtual bool Succeeded { get; protected set; }

        public virtual Exception Error { get; protected set; }
        public virtual IActionMetric Metrics { get; protected set; }
        public virtual string Message { get; protected set; }

        public static IStageResult Fail(string message = "")
        {
            return new Result(message) {Succeeded = false};
        }

        public static IStageValueResult<TValue> Fail<TValue>(string message = "")
        {
            return new Result<TValue> { Message = message, Succeeded = false };
        }

        public static Task<IStageResult> FailAsync(string message = "")
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<IStageValueResult<TValue>> FailAsync<TValue>(string message = "")
        {
            return Task.FromResult(Fail<TValue>(message));
        }


        public static IStageResult Fail(Exception exception, string message = "")
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return new Result(message) {Succeeded = false, Error = exception};
        }

        public static Task<IStageResult> FailAsync(Exception exception, string message = "")
        {
            return Task.FromResult(Fail(exception, message));
        }


        public static IStageValueResult<TValue> Fail<TValue>(Exception exception, string message = "")
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return new Result<TValue> { Succeeded = false, Error = exception, Message = message};
        }

        public static Task<IStageValueResult<TValue>> FailAsync<TValue>(Exception exception, string message = "")
        {
            return Task.FromResult(Fail<TValue>(exception, message));
        }


        public static IStageValueResult<TValue> SuccessWithValue<TValue>(TValue value, string message = "")
        {
            return new Result<TValue>(value) { Succeeded = true, Message = message };
        }

        public static Task<IStageValueResult<TValue>> SuccessWithValueAsync<TValue>(TValue value, string message = "")
        {
            
            return Task.FromResult(SuccessWithValue(value, message));
        }

        public static IStageResult Success(string message = "")
        {
            return new Result(message){ Succeeded=true };
        }

        public static Task<IStageResult> SuccessAsync(string message = "")
        {
            return Task.FromResult(Success(message));
        }

        public override string ToString()
        {
            var str = string.Empty;
            if (Succeeded)
                str += "Success";
            else if (Error == null)
                str += "Failed";
            else
                str += "Failed with an error";

            if (!string.IsNullOrEmpty(Message))
                str += $": {Message}";

            if (Error != null)
                str += $" ({Error.Message})";

            return str;
        }
    }
}