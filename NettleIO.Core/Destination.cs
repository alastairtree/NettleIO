using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public abstract class Destination<TData> : IDestination<TData>
    {
        public abstract Task<IStageResult> SendAsync(TData item);

        //TODO: look at the duplication of sucess and failure methods between Source.cs and Stage.cs
        public virtual Task<IStageResult> SuccessAsync(string message = "")
        {
            return Result.SuccessAsync(message);
        }

        public virtual Task<IStageResult> FailureAsync(Exception exception, string message = "")
        {
            return Task.FromResult(Result.Fail(exception, message));
        }
    }
}