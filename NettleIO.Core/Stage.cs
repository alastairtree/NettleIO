using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public abstract class Stage<TDataIn, TDataOut> : IStage<TDataIn, TDataOut>
    {
        //TODO: Inject a context on preparation of every stage?
        protected IPipelineContext Context { get; set; }

        public abstract Task<IStageValueResult<TDataOut>> Execute(TDataIn input);

        protected virtual IStageValueResult<TDataOut> Success(TDataOut result)
        {
            return Result.SuccessWithValue(result);
        }

        protected virtual IStageValueResult<TDataOut> Failure(Task<TDataOut> failedTask)
        {
            if (failedTask.Exception != null)
                return Failure(failedTask.Exception);

            if (failedTask.IsFaulted)
                return Failure("Task was faulted");

            if (failedTask.IsCanceled)
                return Failure("Task was canceled");

            return Failure("Some unknown failure");
        }

        protected virtual IStageValueResult<TDataOut> Failure(string message = "")
        {
            return Result.Fail<TDataOut>(message);
        }

        protected virtual IStageValueResult<TDataOut> Failure(Exception exception)
        {
            return Result.Fail<TDataOut>(exception);
        }

        protected virtual Task<IStageValueResult<TDataOut>> SuccessAsync(TDataOut value, string message = "")
        {
            return Result.SuccessWithValueAsync(value, message);
        }

        protected virtual Task<IStageValueResult<TDataOut>> FailureAsync(Exception exception, string message = "")
        {
            return Result.FailAsync<TDataOut>(exception, message);
        }

        //TODO: try and remove the need for objects! can we use expressions lamdas and generics to make this more friendly?!

        public async Task<IStageValueResult<object>> Execute(object input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if(input.GetType() != typeof(TDataIn))
                throw new ArgumentOutOfRangeException(nameof(input), $"Expected type {typeof(TDataIn)} but recieved {input.GetType()}.");

            return (IStageValueResult<object>) await Execute((TDataIn) input);
        }
    }


    public abstract class Stage<TData> : Stage<TData, TData>, IStage<TData>, IStage<TData,TData>
    {

    }
}
