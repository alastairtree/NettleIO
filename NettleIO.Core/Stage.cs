using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public abstract class Stage<TDataIn, TDataOut> : IStage<TDataIn, TDataOut>
    {
        //TODO: Inject a context on preparation of every stage?
        protected IPipelineContext Context { get; set; }

        public abstract Task<IValueResult<TDataOut>> Execute(TDataIn input);

        protected virtual IValueResult<TDataOut> Success(string message = "")
        {
            return Result.Success<TDataOut>(message);
        }

        protected virtual IValueResult<TDataOut> Success(TDataOut result)
        {
            return Result<TDataOut>.SuccessWithValue(result);
        }

        protected virtual IValueResult<TDataOut> Failure(Task<TDataOut> failedTask)
        {
            if (failedTask.Exception != null)
                return Failure(failedTask.Exception);

            if (failedTask.IsFaulted)
                return Failure("Task was faulted");

            if (failedTask.IsCanceled)
                return Failure("Task was canceled");

            return Failure("Some unknown failure");
        }

        protected virtual IValueResult<TDataOut> Failure(string message = "")
        {
            return Result<TDataOut>.Fail(message);
        }

        protected virtual IValueResult<TDataOut> Failure(Exception exception)
        {
            return Result<TDataOut>.Fail(exception);
        }

        protected virtual Task<IValueResult<TDataOut>> SuccessAsync(TDataOut value, string message = "")
        {
            return Task.FromResult((IValueResult<TDataOut>)Result.SuccessWithValue(value, message));
        }

        protected virtual Task<IValueResult<TDataOut>> FailureAsync(Exception exception, string message = "")
        {
            return Task.FromResult((IValueResult<TDataOut>)Result.Fail(exception, message));
        }

        //TODO: try and remove the need for objects! can we use expressions lamdas and generics to make this more friendly?!

        public async Task<IValueResult<object>> Execute(object input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if(input.GetType() != typeof(TDataIn))
                throw new ArgumentOutOfRangeException(nameof(input), $"Expected type {typeof(TDataIn)} but recieved {input.GetType()}.");

            return (IValueResult<object>) await Execute((TDataIn) input);
        }
    }


    public abstract class Stage<TData> : Stage<TData, TData>, IStage<TData>, IStage<TData,TData>
    {

    }
}
