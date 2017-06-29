using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public abstract class Destination<TData> : IDestination<TData>
    {
        public abstract Task<IActionResult> SendAsync(TData item);

        //TODO: try and remove the need for object! can we use expressions lamdas and generics to make this more friendly?!
        public async Task<IActionResult> SendAsync(object input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.GetType() != typeof(TData))
                throw new ArgumentOutOfRangeException(nameof(input), $"Expected type {typeof(TData)} but recieved {input.GetType()}.");

            return await SendAsync((TData) input);
        }

        //TODO: look at the duplication of sucess and failure methods between Source.cs and Stage.cs
        public virtual Task<IActionResult> SuccessAsync(string message = "")
        {
            return Task.FromResult((IActionResult)Result.Success(message));
        }

        public virtual Task<IActionResult> FailureAsync(Exception exception, string message = "")
        {
            return Task.FromResult((IActionResult)Result.Fail(exception, message));
        }
    }
}