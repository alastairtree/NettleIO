using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public abstract class Source<TData> : ISource
    { 
        public abstract Task<IValueResult<TData>> RecieveAsync();

        //TODO: look at the duplication of sucess and failure methods between Source.cs and Stage.cs

        public virtual Task<IValueResult<TData>> SuccessAsync(TData value, string message = "")
        {
            return Task.FromResult((IValueResult<TData>) Result.SuccessWithValue(value, message));
        }

        public virtual Task<IValueResult<TData>> FailureAsync(Exception exception, string message = "")
        {
            return Task.FromResult((IValueResult<TData>)Result.Fail(exception, message));
        }

        async Task<IValueResult<object>> ISource.RecieveAsync()
        {
            return (IValueResult<object>) await RecieveAsync();
        }
    }
}