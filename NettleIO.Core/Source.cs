using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public abstract class Source<TData> : ISource<TData>
    { 
        public abstract Task<IStageValueResult<TData>> RecieveAsync();

        //TODO: look at the duplication of sucess and failure methods between Source.cs and Stage.cs

        public virtual Task<IStageValueResult<TData>> SuccessAsync(TData value, string message = "")
        {
            return Task.FromResult(Result.SuccessWithValue(value, message));
        }

        public virtual Task<IStageValueResult<TData>> FailureAsync(Exception exception, string message = "")
        {
            return Task.FromResult(Result.Fail<TData>(exception, message));
        }
    }
}